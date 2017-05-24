using System;
using System.Text;
using System.Threading.Tasks;

using Amazon;
using Amazon.Ec2;
using Amazon.Elb;
using Amazon.Ssm;

using Carbon.Json;
using Carbon.Net;
using Carbon.Platform.Computing;
using Carbon.Platform.Networking;
using Carbon.Platform.Resources;
using Carbon.Platform.Storage;

using Dapper;

namespace Carbon.Platform.Management
{
    using static ResourceProvider;

    public class HostManager
    {
        private readonly Ec2Client ec2;
        private readonly SsmClient ssm;
        private readonly ElbClient elb;

        private readonly IHostService hostService;
        private readonly IClusterService clusterService;
        private readonly IImageService images;

        private readonly PlatformDb db;

        public HostManager(IAwsCredential credential, PlatformDb db)
        {
            #region Precondtions

            if (credential == null)
                throw new ArgumentNullException(nameof(credential));

            #endregion

            this.db = db ?? throw new ArgumentNullException(nameof(db));

            ec2 = new Ec2Client(AwsRegion.USEast1, credential);
            ssm = new SsmClient(AwsRegion.USEast1, credential);
            elb = new ElbClient(AwsRegion.USEast1, credential);

            this.hostService    = new HostService(db);
            this.clusterService = new ClusterService(db);
            this.images  = new ImageService(db);
        }

        public async Task<IHost[]> LaunchHostsAsync(
            IEnvironment env,
            ILocation zone,
            int launchCount = 1)
        {
            #region Preconditions

            if (env == null)
                throw new ArgumentNullException(nameof(env));

            if (zone == null)
                throw new ArgumentNullException(nameof(zone));

            #endregion

            var locationId = LocationId.Create(zone.Id);

            var region = Locations.Get(locationId.WithZoneNumber(0));

            var cluster = await clusterService.GetAsync(env, region).ConfigureAwait(false);

            // TODO:
            // if the location is a region, select the avaiability zone with the least hosts

            if (!cluster.Details.TryGetValue(ClusterProperties.HostTemplateId, out var hostTemplateId))
            {
                throw new Exception("The cluster does not have a host template");
            }

            var template = await db.HostTemplates.FindAsync((long)hostTemplateId);

            return await LaunchHostsAsync(cluster, zone, template, launchCount).ConfigureAwait(false);
        }

        public async Task<IHost[]> LaunchHostsAsync(
            Cluster cluster, 
            ILocation zone, 
            HostTemplate template, 
            int launchCount = 1)
        {
            #region Preconditions

            if (cluster == null)
                throw new ArgumentNullException(nameof(cluster));

            if (zone == null)
                throw new ArgumentNullException(nameof(zone));

            if (template == null)
                throw new ArgumentNullException(nameof(template));

            #endregion

            var zoneId = LocationId.Create(zone.Id);

            if (zoneId.ZoneNumber == 0)
            {
                throw new Exception("Must launch within in availability zone. Was a region:" + zone.Name);
            }

            var region = Locations.Get(zoneId.WithZoneNumber(0));

            var image = await images.GetAsync(template.ImageId).ConfigureAwait(false);

            var machineType = AwsInstanceType.Get(template.MachineTypeId);

            var request = new RunInstancesRequest {
                ClientToken  = Guid.NewGuid().ToString(),
                InstanceType = machineType.Name,
                ImageId      = image.ResourceId,
                MinCount     = launchCount,
                MaxCount     = launchCount,
                Placement    = new Placement(availabilityZone: zone.Name),
                TagSpecifications = new[] {
                    new TagSpecification(
                        resourceType: "instance",
                        tags: new[] { new Amazon.Ec2.Tag("envId", cluster.EnvironmentId.Value.ToString())
                    })
                }
            };

            if (template.StartupScript != null)
            {
                // Open Question: Can we use UTF8?

                request.UserData = Convert.ToBase64String(Encoding.ASCII.GetBytes(template.StartupScript));
            }

            #region AWS Specific Properties

            foreach (var property in template.Details)
            {
                switch (property.Key)
                {
                    case HostTemplateProperties.IamRole:
                        // NOTE: This requires the PassRole permission
                        // https://aws.amazon.com/blogs/security/granting-permission-to-launch-ec2-instances-with-iam-roles-passrole-permission/

                        request.IamInstanceProfile = new IamInstanceProfileSpecification(property.Value);

                        break;
                    case HostTemplateProperties.KernelId:
                        request.KernelId = property.Value;

                        break;

                    case HostTemplateProperties.SecurityGroupIds:
                        request.SecurityGroupIds = property.Value.ToArrayOf<string>();

                        break;

                    case HostTemplateProperties.EBSOptimized:
                        request.EbsOptimized = (bool)property.Value;

                        break;

                    case HostTemplateProperties.Monitoring:
                        request.Monitoring = new RunInstancesMonitoringEnabled((bool)property.Value);

                        break;

                    case HostTemplateProperties.Volume:
                        var volSpec = property.Value.As<VolumeSpec>();

                        // TODO: Device Name
                        request.BlockDeviceMappings = new[] {
                            new BlockDeviceMapping(BlockDeviceNames.Root, new EbsBlockDevice(
                                volumeType: volSpec.Type,
                                volumeSize: (int)volSpec.Size
                            ))
                        };

                        break;

                    case HostTemplateProperties.KeyName:
                        request.KeyName = property.Value;

                        break;
                }
            }

            #endregion

            var runInstancesResponse = await ec2.RunInstancesAsync(request).ConfigureAwait(false);

            var hosts = new IHost[runInstancesResponse.Instances.Length];
            
            for (var i = 0; i < hosts.Length; i++)
            {
                var registerRequest = await GetRegisterHostRequestAsync(
                    instance     : runInstancesResponse.Instances[i],
                    cluster      : cluster,
                    image        : image, 
                    machineType  : machineType, 
                    location     : zone);

                hosts[i] = await hostService.RegisterAsync(registerRequest).ConfigureAwait(false);
            }

            return hosts;
        }

        public async Task TransitionStateAsync(HostInfo host, HostStatus newStatus)
        {
            if (host.Status == HostStatus.Pending && newStatus == HostStatus.Running)
            {
                var cluster = await clusterService.GetAsync(host.ClusterId).ConfigureAwait(false);

                if (cluster.Details.ContainsKey(ClusterProperties.TargetGroupArn))
                {
                    await RegisterWithTargetGroupAsync(host, cluster).ConfigureAwait(false);
                }

                using (var connection = db.Context.GetConnection())
                {
                    await connection.ExecuteAsync(
                        @"UPDATE `Hosts`
                          SET `status` = @status
                          WHERE `id` = @id", host
                    ).ConfigureAwait(false);
                }
            }
        }

        public async Task RegisterWithTargetGroupAsync(HostInfo host, Cluster group)
        {
            var targetRegistration = new RegisterTargetsRequest(
                targetGroupArn : group.Details[ClusterProperties.TargetGroupArn],
                targets        : new[] { new TargetDescription(id: host.ResourceId) }
            );
            
            // Register the instances with the lb's target group
            await elb.RegisterTargetsAsync(targetRegistration);
        }

        public async Task TerminateHostAsync(HostInfo host, TimeSpan cooldown)
        {
            var cluster = await clusterService.GetAsync(host.ClusterId);

            if (cluster.Details.TryGetValue("targetGroupArn", out var targetGroupArn))
            {
                // Degister the instances from the load balancers target group
                await elb.DeregisterTargetsAsync(new DeregisterTargetsRequest(
                    targetGroupArn: targetGroupArn, 
                    targets: new[] {
                        new TargetDescription(host.ResourceId)
                    }
                ));
            }

            // Cooldown to allow the connections to drain
            await Task.Delay(cooldown);

            var request = new TerminateInstancesRequest(host.ResourceId);

            await ec2.TerminateInstancesAsync(request).ConfigureAwait(false);
        }

        public Task<SendCommandResponse> RunCommandAsync(
            string documentName,
            IEnvironment env,
            JsonObject parameters,
            RunCommandOptions options)
        {
            #region Preconditions

            if (documentName == null)
                throw new ArgumentNullException(nameof(documentName));

            if (env == null)
                throw new ArgumentNullException(nameof(env));

            #endregion

            return ssm.SendCommandAsync(new SendCommandRequest(
               documentName : documentName,
               targets      : new[] { new CommandTarget("tag:envId", env.Id.ToString()) }) {
               MaxErrors      = options.MaxErrors,
               MaxConcurrency = options.MaxConcurrency,
               Parameters     = parameters
            });
        }

        // arn:aws:elasticloadbalancing:{region}:{accountId}:targetgroup/{groupName}/{groupId}
        
        private async Task<IHost> RegisterAsync(string instanceId, Cluster cluster)
        {
            #region Preconditions

            if (instanceId == null)
                throw new ArgumentNullException(nameof(instanceId));

            #endregion

            var ec2Instance = await ec2.DescribeInstanceAsync(instanceId).ConfigureAwait(false) 
                ?? throw new ResourceNotFoundException(resource: $"aws:host/{instanceId}");

            var request = await GetRegisterHostRequestAsync(ec2Instance, cluster);

            return await hostService.RegisterAsync(request).ConfigureAwait(false);
        }

        private async Task<RegisterHostRequest> GetRegisterHostRequestAsync(
            Instance instance,
            Cluster cluster,
            IImage image = null,
            ILocation location = null,
            IMachineType machineType = null)
        {
            #region Preconditions

            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            // Forbid classic instances by ensuring we're inside of a VPC
            if (instance.VpcId == null)
                throw new ArgumentException("Must belong to a VPC", nameof(instance));

            #endregion

            #region Data Binding / Mappings

            if (location == null)
            {
                location = Locations.Get(Aws, instance.Placement.AvailabilityZone);
            }

            if (image == null)
            {
                // "imageId": "ami-1647537c",
                
                image = await images.GetAsync(Aws, instance.ImageId).ConfigureAwait(false);
            }

            if (machineType == null)
            {
                machineType = AwsInstanceType.Get(instance.InstanceType);
            }

            var network = await db.Networks.FindAsync(Aws, instance.VpcId).ConfigureAwait(false);

            #endregion

            // instance.LaunchTime

            int addressCount = 1;

            if (instance.IpAddress != null)
            {
                addressCount++;
            }

            var addresses = new string[addressCount];

            addresses[0] = instance.PrivateIpAddress;
            
            if (instance.IpAddress != null)
            {
                // the instance was assigned a public IP
                addresses[1] = instance.IpAddress;
            }
            
            var registerRequest = new RegisterHostRequest(
                addresses    : addresses,
                cluster      : cluster,
                location     : location,
                image : image,
                machineType  : machineType,
                status       : instance.InstanceState.ToStatus(),
                ownerId      : 1,
                resource     : ManagedResource.Host(location, instance.InstanceId)                
            );

            #region Network Interfaces

            try
            {
                var nics = new RegisterNetworkInterfaceRequest[instance.NetworkInterfaces.Length];

                for (var nicIndex = 0; nicIndex < nics.Length; nicIndex++)
                {
                    var ec2Nic = instance.NetworkInterfaces[nicIndex];

                    nics[nicIndex] = new RegisterNetworkInterfaceRequest(
                        mac: MacAddress.Parse(ec2Nic.MacAddress),
                        subnetId: 0,                   // TODO: lookup subnet
                        securityGroupIds: Array.Empty<long>(), // TODO: lookup security groupds
                        resource: ManagedResource.NetworkInterface(location, ec2Nic.NetworkInterfaceId)
                    );
                }

                registerRequest.NetworkInterfaces = nics;
            }
            catch { }

            #endregion

            #region Volumes

            try
            {
                var volumes = new RegisterVolumeRequest[instance.BlockDeviceMappings.Length];

                for (var volumeIndex = 0; volumeIndex < volumes.Length; volumeIndex++)
                {
                    var device = instance.BlockDeviceMappings[volumeIndex];

                    if (device.Ebs == null) continue;

                    var volumeSize = device.Ebs.VolumeSize is int ebsSize
                        ? ByteSize.GiB(ebsSize)
                        : ByteSize.Zero;

                    volumes[volumeIndex] = new RegisterVolumeRequest(
                        size: volumeSize,
                        resource: ManagedResource.Volume(location, device.Ebs.VolumeId),
                        ownerId: 1
                    );
                }

                registerRequest.Volumes = volumes;
            }
            catch { }

            #endregion

            return registerRequest;
        }
    }
}
