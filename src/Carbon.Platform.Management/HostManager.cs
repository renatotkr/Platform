using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Amazon;
using Amazon.Ec2;
using Amazon.Elb;
using Amazon.Ssm;

using Carbon.Json;
using Carbon.Platform.Computing;
using Carbon.Platform.Resources;
using Carbon.Platform.Services;

namespace Carbon.Platform.Management
{
    public class HostManager
    {
        private readonly Ec2Client ec2;
        private readonly SsmClient ssm;
        private readonly ElbClient elb;

        private readonly HostService hostService;
        private readonly VolumeManager volumeManager;
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

            this.hostService   = new HostService(db);
            this.volumeManager = new VolumeManager(db, ec2);
        }

        public async Task LaunchHostsAsync(IEnvironment env, ILocation location, int launchCount = 1)
        {
            var locationId = LocationId.Create(location.Id);

            var region = Locations.Get(locationId.WithZoneNumber(0));

            var group = await hostService.GetGroupAsync(env, region).ConfigureAwait(false);

            // if the location is a region, select the avaiability zone with the least hosts

            var template = await db.HostTemplates.FindAsync(group.HostTemplateId.Value);

            await LaunchHostsAsync(env, location, template, launchCount).ConfigureAwait(false);
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
                    new TagSpecification("instance", tags: new[] { new Amazon.Ec2.Tag("envId", env.Id.ToString()) })
                }
            };
            


            if (template.StartupScript != null)
            {
                // $UserData = [System.Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes($Script))

                request.UserData = Convert.ToBase64String(Encoding.ASCII.GetBytes(template.StartupScript));
            }

            // AWS Specific Configiration Properties
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

            var group = await hostService.GetGroupAsync(env, region).ConfigureAwait(false);

            if (group == null)
            {
                throw new Exception("No group found in region");
            }

            if (!group.Details.ContainsKey(HostGroupProperties.TargetGroupArn))
            {
                throw new ArgumentNullException("host group does not have a targetGroupArn property");
            }

            var runResult = await ec2.RunInstancesAsync(request);

            var hosts = new IHost[runResult.Instances.Length];
            
            for(var i = 0; i < runResult.Instances.Length; i++)
            {
                var instance = runResult.Instances[i];

                /*
                if (instance.IpAddress == null || instance.InstanceId == null)
                {
                    throw new Exception(JsonObject.FromObject(instance).ToString());
                }
                */

                var createRequest = new CreateHostRequest(
                    addresses    : new[] { IPAddress.Parse(instance.PrivateIpAddress) },
                    env          : env,
                    groupId      : group.Id,
                    location     : location,
                    machineImage : machineImage,
                    machineType  : machineType,
                    status       : HostStatus.Pending,
                    resource     : ManagedResource.Host(location, instance.InstanceId)                
                );

                hosts[i] = await hostService.CreateAsync(createRequest).ConfigureAwait(false);
            }

            return hosts;
        }

        public async Task TransitionStateAsync(HostInfo host, HostStatus status)
        {
            if (host.Status == HostStatus.Pending && status == HostStatus.Running)
            {
                if (host.GroupId != null)
                {
                    var group = await hostService.GetGroupAsync(host.GroupId.Value).ConfigureAwait(false);

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
                var group = await hostService.GetGroupAsync(host.GroupId.Value);

                if (group.Details.TryGetValue("targetGroupArn", out var targetGroupArn))
                {
                    // Register the instances with the lb's target group
                    await elb.DeregisterTargetsAsync(new DeregisterTargetsRequest(targetGroupArn, new[] { new TargetDescription(host.ResourceId) }));
                }

                // Cooldown to allow the connections to drain
                await Task.Delay(cooldown);
            }

            var request = new TerminateInstancesRequest(host.ResourceId);

            await ec2.TerminateInstancesAsync(request).ConfigureAwait(false);
        }


        public Task<SendCommandResponse> RunCommandAsync(IEnvironment env, string documentName, JsonObject parameters)
        {
            // TODO: Lookup latest hash...

           return ssm.SendCommandAsync(new SendCommandRequest(
               documentName : documentName,
               targets      : new[] { new CommandTarget("envId", env.Id.ToString()) }) {
                MaxErrors      = "50%",
                MaxConcurrency = "50%",
                Parameters     = parameters,
            });
        }

        // arn:aws:elasticloadbalancing:{region}:{accountId}:targetgroup/{groupName}/{groupId}
        
        private async Task<IHost> RegisterAsync(string instanceId, IEnvironment env)
        {
            #region Preconditions

            if (instanceId == null)
                throw new ArgumentNullException(nameof(instanceId));

            #endregion

            var ec2Instance = await ec2.DescribeInstanceAsync(instanceId).ConfigureAwait(false) ?? throw new Exception($"Instance {instanceId} not found");

            if (ec2Instance.VpcId == null) throw new Exception("Instancem must reside inside of a VPC");

            var network = await db.Networks.FindAsync(ResourceProvider.Aws, ec2Instance.VpcId).ConfigureAwait(false);

            // "imageId": "ami-1647537c",

            var image = ec2Instance.ImageId != null ? await hostService.GetMachineImageAsync(ResourceProvider.Aws, ec2Instance.ImageId).ConfigureAwait(false) : null;

            var location = Locations.Get(ResourceProvider.Aws, ec2Instance.Placement.AvailabilityZone);

            long machineTypeId = AwsInstanceType.GetId(ec2Instance.InstanceType);

            var addresses = new[] {
                IPAddress.Parse(ec2Instance.PrivateIpAddress),
                IPAddress.Parse(ec2Instance.IpAddress)
            };

            // instance.LaunchTime

            var host = await hostService.CreateAsync(new CreateHostRequest {
                Addresses       = addresses,
                Location        = location,
                Status          = ec2Instance.InstanceState.ToStatus(),
                MachineTypeId   = machineTypeId,
                EnvironmentId   = env.Id,
                Resource        = ManagedResource.Host(location, ec2Instance.InstanceId),
                MachineImageId  = image.Id,
                NetworkId       = network?.Id ?? 0
            }).ConfigureAwait(false);
            
          
            foreach (var v in ec2Instance.BlockDeviceMappings)
            {
                if (v.Ebs == null) continue;

                var volume = await volumeManager.GetAsync(ResourceProvider.Aws, v.Ebs.VolumeId, host).ConfigureAwait(false);
            }


            return host;

            /*
            foreach (var ec2NetworkInterface in instance.NetworkInterfaces)
            {
                if (ec2NetworkInterface.VpcId != instance.VpcId)
                {
                    throw new Exception("Host's network interface belongs to different VPC:" + ec2NetworkInterface.VpcId);
                }

                var networkInterface = await networkService.ConfigureEc2NetworkInterfaceAsync(ec2NetworkInterface).ConfigureAwait(false);

                networkInterface.HostId = host.Id;

                await db.NetworkInterfaces.InsertAsync(networkInterface).ConfigureAwait(false);
            }
            */
        }
    }
}
