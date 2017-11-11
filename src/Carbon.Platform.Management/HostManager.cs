using System;
using System.Text;
using System.Threading.Tasks;

using Amazon;
using Amazon.Ec2;
using Amazon.Elb;
using Amazon.Ssm;

using Carbon.Cloud.Logging;
using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Json;
using Carbon.Net;
using Carbon.Platform.Computing;
using Carbon.Platform.Environments;
using Carbon.Platform.Networking;
using Carbon.Platform.Resources;
using Carbon.Platform.Storage;
using Carbon.Security;

namespace Carbon.Platform.Management
{
    using static Expression;

    public class HostManager
    {
        private readonly Ec2Client ec2;
        private readonly SsmClient ssm;
        private readonly ElbClient elb;

        private readonly IHostService hostService;
        private readonly IHostTemplateService hostTemplateService;
        private readonly IClusterService clusterService;
        private readonly IImageService imageService;
        private readonly IEventLogger eventLog;
        private readonly PlatformDb db;

        public HostManager(IAwsCredential credential, PlatformDb db, IEventLogger eventLog)
        {
            if (credential == null)
                throw new ArgumentNullException(nameof(credential));

            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.eventLog = eventLog ?? throw new ArgumentNullException(nameof(eventLog));

            var region = AwsRegion.USEast1; // TODO: Configurable

            ec2 = new Ec2Client(region, credential);
            ssm = new SsmClient(region, credential);
            elb = new ElbClient(region, credential);

            this.hostService         = new HostService(db);
            this.clusterService      = new ClusterService(db);
            this.imageService        = new ImageService(db);
            this.hostTemplateService = new HostTemplateService(db);
        }

        /*
        public async Task<HostInfo> GetAsync(ResourceProvider provider, string resourceId)
        {
            var ec2Instance = await ec2.DescribeInstanceAsync(instanceId)
             ?? throw ResourceError.NotFound(ResourceProvider.Aws, ResourceTypes.Host, instanceId);

            var request = await GetRegistrationAsync(ec2Instance, cluster);

            return await hostService.RegisterAsync(request); ;
        }
        */
        
        // TODO: Accept bash script?

        public async Task RunCommandAsync(string commandText, IEnvironment environment, ISecurityContext context)
        {
            #region Preconditions

            if (commandText == null)
                throw new ArgumentNullException(nameof(commandText));

            if (environment == null)
                throw new ArgumentNullException(nameof(environment));

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            #endregion

            var commands = CommandHelper.ToLines(commandText);

            var parameters = new JsonObject {
                { "commands", commands }
            };
            
            // tag: || InstanceIds
            // if tag: value:WebServer

            var request = new SendCommandRequest {
                DocumentName = "AWS-RunShellScript",
                Targets      = new[] { new CommandTarget("tag:envId", values: new[] { environment.Id.ToString() }) },
                Parameters   = parameters
            };

            var result = await ssm.SendCommandAsync(request);

            #region Logging

            await eventLog.CreateAsync(new Event(
                action   : "run:command",
                resource : "environment#" + environment.Id,
                userId   : context.UserId.Value,
                context  : new JsonObject
                {
                    {  "commandId", result.Command.CommandId }
                }
            ));

            #endregion
        }

        public async Task<HostInfo> RegisterAsync(RegisterHostRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var provider = ResourceProvider.Get(request.Resource.ProviderId);

            var host = await hostService.FindAsync(provider, request.Resource.ResourceId);;
            
            if (host == null)
            {
                // Ensure the cluster exists
                var cluster = request.ClusterId != 0
                    ? await clusterService.GetAsync(request.ClusterId)
                    : null;

                host = await hostService.RegisterAsync(request);
            }
            else
            {
                await TransitionStateAsync(host, request.Status);;
            }

            return host;
        }

        public async Task<IHost[]> LaunchAsync(Cluster cluster, ISecurityContext context)
        {
            if (cluster == null)
                throw new ArgumentNullException(nameof(cluster));

            var zoneId = LocationId.Create(cluster.LocationId).WithZoneNumber(1);

            var zone = Locations.Get(zoneId);

            // TODO:
            // if the location is a region, balance hosts within available zones

            var template = await hostTemplateService.GetAsync(cluster.HostTemplateId);

            var request = new LaunchHostRequest(cluster, zone, template);

            return await LaunchAsync(request, context);;
        }

        public async Task<IHost[]> LaunchAsync(
            LaunchHostRequest launchRequest,
            ISecurityContext context)
        {
            #region Preconditions

            if (launchRequest == null)
                throw new ArgumentNullException(nameof(launchRequest));

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            #endregion

            var zoneId = LocationId.Create(launchRequest.Location.Id);

            if (zoneId.ZoneNumber == 0)
            {
                throw new Exception("Must launch within in availability zone. Was a region:" + launchRequest.Location.Name);
            }

            var cluster  = launchRequest.Cluster;
            var zone     = launchRequest.Location;
            var template = launchRequest.Template;
            
            var region = Locations.Get(zoneId.WithZoneNumber(0));

            var image = await imageService.GetAsync(template.ImageId);;

            var machineType = AwsInstanceType.Get(template.MachineTypeId);

            var request = new RunInstancesRequest {
                ClientToken  = Guid.NewGuid().ToString(),
                InstanceType = machineType.Name,
                ImageId      = image.ResourceId,
                MinCount     = launchRequest.LaunchCount,
                MaxCount     = launchRequest.LaunchCount,
                Placement    = new Placement(availabilityZone: zone.Name),
                TagSpecifications = new[] {
                    new TagSpecification(
                        resourceType : "instance",
                        tags         : new[] { new Amazon.Ec2.Tag("envId", cluster.EnvironmentId.ToString()) }
                    )
                }
            };

            var startupScript = launchRequest.StartupScript ?? template.StartupScript;

            if (startupScript != null)
            {
                request.UserData = Convert.ToBase64String(Encoding.UTF8.GetBytes(startupScript));
            }

            #region AWS Specific Properties

            foreach (var property in template.Properties)
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

                    case HostTemplateProperties.EbsOptimized:
                        request.EbsOptimized = (bool)property.Value;

                        break;

                    case HostTemplateProperties.Monitoring:
                        request.Monitoring = new RunInstancesMonitoringEnabled((bool)property.Value);

                        break;

                    case HostTemplateProperties.Volume:
                        var volSpec = property.Value.As<AwsVolumeSpecification>();

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

            var runInstancesResponse = await ec2.RunInstancesAsync(request);;

            var hosts = new IHost[runInstancesResponse.Instances.Length];
            
            for (var i = 0; i < hosts.Length; i++)
            {
                var registerRequest = await GetRegistrationAsync(
                    instance    : runInstancesResponse.Instances[i],
                    cluster     : cluster,
                    image       : image, 
                    machineType : machineType, 
                    location    : zone
                );

                hosts[i] = await hostService.RegisterAsync(registerRequest);;
            }

            #region Logging

            await eventLog.CreateAsync(new Event(
                action   : "launch",
                resource : "hosts",
                userId   : context.UserId)
            );;

            #endregion

            return hosts;
        }

        public async Task TransitionStateAsync(HostInfo host, HostStatus newStatus)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
            
            if (host.Status == HostStatus.Pending && newStatus == HostStatus.Running)
            {
                var cluster = await clusterService.GetAsync(host.ClusterId);;

                if (cluster.Properties.ContainsKey(ClusterProperties.TargetGroupArn))
                {
                    await RegisterWithTargetGroupAsync(host, cluster);;
                }
                
                await db.Hosts.PatchAsync(host.Id, new[] {
                    Change.Replace("status", newStatus)
                });;

                await eventLog.CreateAsync(new Event(
                    action   : "update",
                    resource : "host#" + host.Id,
                    properties: JsonObject.FromObject(new {
                        changes = new {
                            status = new {
                                oldValue = host.Status.ToString(),
                                newValue = newStatus.ToString()
                            }
                        }
                    })
                ));;
            }
        }

        public async Task RegisterWithTargetGroupAsync(HostInfo host, Cluster group)
        {
            #region Preconditions

            if (host == null)
                throw new ArgumentNullException(nameof(host));

            if (group == null)
                throw new ArgumentNullException(nameof(group));

            #endregion


            var registration = new RegisterTargetsRequest(
                targetGroupArn : group.Properties[ClusterProperties.TargetGroupArn],
                targets        : new[] { new TargetDescription(id: host.ResourceId) }
            );
            
            // Register the instances with the lb's target group
            await elb.RegisterTargetsAsync(registration);

            await eventLog.CreateAsync(new Event(
                action   : "addToLoadBalancerTargetGroup",
                resource : "host#" + host.Id
            ));;
        }

        public async Task TerminateAsync(HostInfo host, TimeSpan cooldown, ISecurityContext context)
        {
            #region Preconditions

            if (host == null)
                throw new ArgumentNullException(nameof(host));

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            #endregion

            if (host.ClusterId > 0)
            {
                var cluster = await clusterService.GetAsync(host.ClusterId);

                if (cluster.Properties != null && 
                    cluster.Properties.TryGetValue(ClusterProperties.TargetGroupArn, out var targetGroupArn))
                {
                    // Degister the instances from the load balancers target group
                    await elb.DeregisterTargetsAsync(new DeregisterTargetsRequest(
                        targetGroupArn: targetGroupArn,
                        targets: new[] {
                            new TargetDescription(host.ResourceId)
                        }
                    ));

                    await eventLog.CreateAsync(new Event(
                        action     : "drain",
                        resource   : "host#" + host.Id,
                        userId     : context.UserId,
                        properties : new JsonObject {
                            { "duration", cooldown.ToString() }
                        })
                    );

                    // Cooldown to allow the connections to drain from the load balancer before issuing the termination command
                    await Task.Delay(cooldown);
                }
            }

            var request = new TerminateInstancesRequest(host.ResourceId);

            await ec2.TerminateInstancesAsync(request);;
            
            // Mark the host as terminated
            await db.Hosts.PatchAsync(host.Id, new[] {
                Change.Replace("terminated", Func("NOW"))
            }, IsNull("terminated"));

            #region Logging

            await eventLog.CreateAsync(new Event(
                action   : "terminate",
                resource : "host#" + host.Id,
                userId   : context.UserId)
            );

            #endregion
        }

        public Task<SendCommandResponse> RunCommandAsync(
            string documentName,
            IEnvironment environment,
            JsonObject parameters,
            RunCommandOptions options)
        {
            #region Preconditions

            if (documentName == null)
                throw new ArgumentNullException(nameof(documentName));

            if (environment == null)
                throw new ArgumentNullException(nameof(environment));

            #endregion

            return ssm.SendCommandAsync(new SendCommandRequest(
               documentName : documentName,
               targets      : new[] { new CommandTarget("tag:envId", environment.Id.ToString()) }) {
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

            if (cluster == null)
                throw new ArgumentNullException(nameof(cluster));

            #endregion

            var ec2Instance = await ec2.DescribeInstanceAsync(instanceId)
                ?? throw ResourceError.NotFound(ResourceProvider.Aws, ResourceTypes.Host, instanceId);

            var request = await GetRegistrationAsync(ec2Instance, cluster);

            return await hostService.RegisterAsync(request);;
        }

        private static readonly ResourceProvider aws = ResourceProvider.Aws;

        private async Task<RegisterHostRequest> GetRegistrationAsync(
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
                location = Locations.Get(aws, instance.Placement.AvailabilityZone);
            }

            if (image == null)
            {
                // "imageId": "ami-1647537c",
                
                image = await imageService.GetAsync(aws, instance.ImageId);;
            }

            if (machineType == null)
            {
                machineType = AwsInstanceType.Get(instance.InstanceType);
            }

            var network = await db.Networks.FindAsync(aws, instance.VpcId);;

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
                addresses   : addresses,
                cluster     : cluster,
                image       : image,
                machineType : machineType,
                program     : null,
                location    : location,
                status      : instance.InstanceState.ToStatus(),
                ownerId     : 1,
                resource    : ManagedResource.Host(location, instance.InstanceId)                
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
                        ? ByteSize.FromGiB(ebsSize)
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
