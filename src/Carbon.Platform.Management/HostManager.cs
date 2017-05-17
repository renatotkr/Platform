using System;
using System.Collections.Generic;
using System.Net;
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

namespace Carbon.Platform.Management
{
    using static ResourceProvider;

    public class HostManager
    {
        private readonly Ec2Client ec2;
        private readonly SsmClient ssm;
        private readonly ElbClient elb;

        private readonly IHostService hostService;
        private readonly IHostGroupService groups;

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

            this.hostService = new HostService(db);
            this.groups = new HostGroupService(db);
        }

        public async Task<IHost[]> LaunchHostsAsync(IEnvironment env, ILocation location, int launchCount = 1)
        {
            var locationId = LocationId.Create(location.Id);

            var region = Locations.Get(locationId.WithZoneNumber(0));

            var group = await groups.GetAsync(env, region).ConfigureAwait(false);

            // TODO:
            // if the location is a region, select the avaiability zone with the least hosts

            var template = await db.HostTemplates.FindAsync(group.HostTemplateId.Value);

            return await LaunchHostsAsync(env, location, template, launchCount).ConfigureAwait(false);
        }

        public async Task<IHost[]> LaunchHostsAsync(
            IEnvironment env, 
            ILocation location, 
            HostTemplate template, 
            int launchCount = 1)
        {
            #region Preconditions

            if (env == null)
                throw new ArgumentNullException(nameof(env));

            if (location == null)
                throw new ArgumentNullException(nameof(location));

            if (template == null)
                throw new ArgumentNullException(nameof(template));

            #endregion

            var locationId = LocationId.Create(location.Id);

            if (locationId.ZoneNumber == 0)
            {
                throw new Exception("Must launch within in availability zone");
            }

            var region = Locations.Get(locationId.WithZoneNumber(0));

            var machineImage = await db.MachineImages.FindAsync(template.MachineImageId) 
                ?? throw new Exception($"machineImage#{template.MachineImageId} not found");

            var machineType = AwsInstanceType.Get(template.MachineTypeId);

            var request = new RunInstancesRequest {
                ClientToken  = Guid.NewGuid().ToString(),
                InstanceType = machineType.Name,
                ImageId      = machineImage.ResourceId,
                MinCount     = launchCount,
                MaxCount     = launchCount,
                Placement    = new Placement(availabilityZone: location.Name),
                TagSpecifications = new[] {
                    new TagSpecification(
                        resourceType: "instance",
                        tags: new[] { new Amazon.Ec2.Tag("envId", env.Id.ToString())
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
                        var volSpec = property.Value.As<VolumeSpecification>();

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

            var group = await groups.GetAsync(env, region).ConfigureAwait(false);

            var runResult = await ec2.RunInstancesAsync(request).ConfigureAwait(false);

            var hosts = new IHost[runResult.Instances.Length];
            
            for (var i = 0; i < runResult.Instances.Length; i++)
            {
                var registerRequest = await GetRegisterHostRequestAsync(runResult.Instances[i], env, machineImage, machineType, location, group);

                hosts[i] = await hostService.RegisterAsync(registerRequest).ConfigureAwait(false);
            }

            return hosts;
        }

        public async Task TransitionStateAsync(HostInfo host, HostStatus status)
        {
            if (host.Status == HostStatus.Pending && status == HostStatus.Running)
            {
                if (host.GroupId != null)
                {
                    var group = await groups.GetAsync(host.GroupId.Value).ConfigureAwait(false);

                    await RegisterHostToGroupAsync(host, group).ConfigureAwait(false);
                }
            }
        }

        public async Task RegisterHostToGroupAsync(HostInfo host, HostGroup group)
        {
            var targetRegistration = new RegisterTargetsRequest(
                targetGroupArn : group.Details[HostGroupProperties.TargetGroupArn],
                targets        : new[] { new TargetDescription(id: host.ResourceId) }
            );
            
            // Register the instances with the lb's target group
            await elb.RegisterTargetsAsync(targetRegistration);
        }

        public async Task TerminateHostAsync(HostInfo host, TimeSpan cooldown)
        {
            if (host.GroupId != null)
            {
                var group = await groups.GetAsync(host.GroupId.Value);

                if (group.Details.TryGetValue("targetGroupArn", out var targetGroupArn))
                {
                    // Register the instances with the lb's target group
                    await elb.DeregisterTargetsAsync(new DeregisterTargetsRequest(targetGroupArn, new[] {
                        new TargetDescription(host.ResourceId)
                    }));
                }

                // Cooldown to allow the connections to drain
                await Task.Delay(cooldown);
            }

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
        
        private async Task<IHost> RegisterAsync(string instanceId, IEnvironment env)
        {
            #region Preconditions

            if (instanceId == null)
                throw new ArgumentNullException(nameof(instanceId));

            #endregion

            var ec2Instance = await ec2.DescribeInstanceAsync(instanceId).ConfigureAwait(false) 
                ?? throw new ResourceNotFoundException(resource: $"aws:host/{instanceId}");

            var request = await GetRegisterHostRequestAsync(ec2Instance, env);

            return await hostService.RegisterAsync(request).ConfigureAwait(false);
        }

        private async Task<RegisterHostRequest> GetRegisterHostRequestAsync(
            Instance instance,
            IEnvironment env,
            IMachineImage machineImage = null,
            IMachineType machineType = null,
            ILocation location = null, 
            IHostGroup group = null)
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

            if (machineImage == null)
            {
                // "imageId": "ami-1647537c",

                machineImage = await new MachineImageService(db).GetAsync(Aws, instance.ImageId).ConfigureAwait(false);
            }

            if (machineType == null)
            {
                machineType = AwsInstanceType.Get(instance.InstanceType);
            }

            var network = await db.Networks.FindAsync(Aws, instance.VpcId).ConfigureAwait(false);

            #endregion

            // instance.LaunchTime

            var addresses = new List<string>();

            addresses.Add(instance.PrivateIpAddress);

            // If the instance was assigned a public IP
            if (instance.IpAddress != null)
            {
                addresses.Add(instance.IpAddress);
            }
            
            var registerRequest = new RegisterHostRequest(
                addresses    : addresses.ToArray(),
                env          : env,
                groupId      : group?.Id,
                location     : location,
                machineImage : machineImage,
                machineType  : machineType,
                status       : instance.InstanceState.ToStatus(),
                resource     : ManagedResource.Host(location, instance.InstanceId)                
            );

            #region Network Interfaces

            if (instance.NetworkInterfaces != null)
            {
                var nics = new RegisterNetworkInterfaceRequest[instance.BlockDeviceMappings.Length];

                for (var ni = 1; ni < instance.NetworkInterfaces.Length; ni++)
                {
                    var ec2Nic = instance.NetworkInterfaces[ni];

                    // TODO: Lookup the subnet

                    // ec2Nic.SubnetId;

                    nics[ni] = new RegisterNetworkInterfaceRequest(
                        mac      : MacAddress.Parse(ec2Nic.MacAddress),
                        subnetId : 0, // TODO
                        resource : ManagedResource.NetworkInterface(location, ec2Nic.NetworkInterfaceId)
                    );
                }

                registerRequest.NetworkInterfaces = nics;
            }

            #endregion

            #region Volumes

            if (instance.BlockDeviceMappings != null)
            {
                var volumes = new RegisterVolumeRequest[instance.NetworkInterfaces.Length];

                for (var bi = 1; bi < instance.BlockDeviceMappings.Length; bi++)
                {
                    var blockDevice = instance.BlockDeviceMappings[bi];

                    if (blockDevice.Ebs == null) continue;

                    var volumeSize = blockDevice.Ebs.VolumeSize != null
                        ? ByteSize.GiB(blockDevice.Ebs.VolumeSize.Value)
                        : ByteSize.Zero;

                    volumes[bi] = new RegisterVolumeRequest(
                        size: volumeSize,
                        resource: ManagedResource.Volume(location, blockDevice.Ebs.VolumeId)
                    );
                }

                registerRequest.Volumes = volumes;
            }

            #endregion

            return registerRequest;
        }
    }
}
