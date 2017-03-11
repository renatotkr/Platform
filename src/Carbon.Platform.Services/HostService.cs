using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

using ec2 = Amazon.Ec2;

namespace Carbon.Platform.Services
{
    using Apps;
    using Computing;
    using Json;
    using Storage;
    using Networking;

    using static Data.Expressions.Expression;

    public class HostService
    {
        private readonly PlatformDb db;
        private readonly ec2::Ec2Client ec2;

        public HostService(PlatformDb db, ec2::Ec2Client ec2)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.ec2 = ec2;
        }

        // Update Hearbeat

        public Task<HostInfo> GetAsync(long id) => db.Hosts.FindAsync(id);
        
        public async Task<HostInfo> GetAsync(CloudResourceInfo resource)
        {
            var host = await db.Hosts.FindAsync(resource.Provider, resource.Name).ConfigureAwait(false);

           return host ?? await GetEc2Instance(resource.Name).ConfigureAwait(false);
        }

        public Task<IReadOnlyList<AppInstance>> GetAppInstancesAsync(long hostId) => 
            db.AppInstances.QueryAsync(Eq("hostId", hostId));
        
        public Task<IReadOnlyList<NetworkInterfaceInfo>> GetNetworkInterfacesAsync(long hostId) => 
            db.NetworkInterfaces.QueryAsync(Eq("hostId", hostId));

        public Task<IReadOnlyList<VolumeInfo>> GetVolumesAsync(long hostId) => 
            db.Volumes.QueryAsync(Eq("hostId", hostId));

        #region EC2 Helpers

        private static HostStatus GetStatus(ec2::InstanceState state)
        {
            switch (state.Name)
            {
                case "pending"       : return HostStatus.Pending;
                case "running"       : return HostStatus.Running;
                case "shutting-down" : return HostStatus.Terminating;
                case "terminated"    : return HostStatus.Terminated;
                case "stopping"      : return HostStatus.Suspending;
                case "stopped"       : return HostStatus.Suspended;

                default: throw new Exception("unexpected state:" + state.Name);
            }
        }

        private async Task<HostInfo> GetEc2Instance(string id)
        {
            var provider = CloudProvider.Amazon;

            var instance = await ec2.DescribeInstanceAsync(id).ConfigureAwait(false);

            if (instance == null)
                throw new Exception($"Instance {id} not found");

            if (instance.VpcId == null)
                throw new Exception("Ec2 instance MUST be inside a VPC");

            var network = await GetNetworkAsync(provider, instance.VpcId).ConfigureAwait(false);

            // "imageId": "ami-1647537c",

            var image = instance.ImageId != null ? await GetImageAsync(provider, instance.ImageId).ConfigureAwait(false) : null;
            var location = Locations.Get(provider, instance.Placement.AvailabilityZone);

            var host = new HostInfo(HostType.Virtual) {
                Status = GetStatus(instance.InstanceState),
                Addresses = new List<IPAddress>() {
                    IPAddress.Parse(instance.IpAddress),
                    IPAddress.Parse(instance.PrivateIpAddress)
                },
                Details = new JsonObject {
                    { "instanceType", instance.InstanceType },
                    { "hypervisor", instance.Hypervisor }
                },
                ProviderId   = provider.Id,
                Name         = instance.InstanceId,
                LocationId   = location.Id,
                NetworkId    = network.Id,
                ImageId      = image?.Id ?? 0,
                Modified     = DateTime.UtcNow,
                Created      = instance.LaunchTime
            };

            await db.Hosts.InsertAsync(host).ConfigureAwait(false);

            foreach (var v in instance.BlockDeviceMappings)
            {
                if (v.Ebs == null) continue;

                var volume = await GetVolumeAsync(provider, v.Ebs.VolumeId).ConfigureAwait(false);
            }

            foreach (var ec2NetworkInterface in instance.NetworkInterfaces)
            {
                if (ec2NetworkInterface.VpcId != instance.VpcId)
                {
                    throw new Exception("Host's network interface belongs to different VPC:" + ec2NetworkInterface.VpcId);
                }

                var networkInterface = NetworkInterfaceFromEc2(ec2NetworkInterface);

                networkInterface.NetworkId = network.Id;
                networkInterface.HostId = host.Id;

                await db.NetworkInterfaces.InsertAsync(networkInterface).ConfigureAwait(false);
            }

            return host;
        }

        public async Task<NetworkInterfaceInfo> GetNetworkInterface(CloudProvider provider, string name)
        {
            // A host may reuse an elastic network interface.... check to see if it exists first...
            // A network interface may also be attached to a load balancer or ??

            var networkInterface = await db.NetworkInterfaces.FindAsync(provider, name).ConfigureAwait(false);

            if (networkInterface == null)
            {
                var n = await ec2.DescribeNetworkInterfaceAsync(name).ConfigureAwait(false);

                var host = await db.Hosts.FindAsync(provider, n.Attachment.InstanceId);

                var network = await GetNetworkAsync(provider, n.VpcId);

              
                if (n.Groups != null)
                {

                    foreach (var group in n.Groups)
                    {
                        var acl = await db.NetworkAcls.FindAsync(provider, group.GroupId).ConfigureAwait(false);

                        if (acl == null)
                        {
                            acl = new NetworkAcl {
                                ProviderId = provider.Id,
                                Name = group.GroupId,
                                Description = group.GroupName,
                                NetworkId = network.Id
                            };

                            await db.NetworkAcls.InsertAsync(acl).ConfigureAwait(false);
                        }

                    }
                }
           

                // ? IP addresses
                // TODO: Subnet

                var ni = NetworkInterfaceFromEc2(n);

                ni.NetworkId = network.Id;
                ni.HostId = host.Id;

                await db.NetworkInterfaces.InsertAsync(ni);
            }

            return networkInterface;
        }

        public static NetworkInterfaceInfo NetworkInterfaceFromEc2(ec2.NetworkInterface ni)
        {
            var networkInterface = new NetworkInterfaceInfo  {
                ProviderId = CloudProvider.Amazon.Id,
                Name       = ni.NetworkInterfaceId,
                MacAddress = ni.MacAddress,
                Details    = new JsonObject { },
                Created    = ni.Attachment?.AttachTime ?? DateTime.UtcNow
            };

            if (ni.SubnetId != null)
            {
                networkInterface.SubnetId = null; // TODO: Lookup

                networkInterface.Details.Add("subnetId", ni.SubnetId);
            }

            return networkInterface;
        }

        public async Task<ImageInfo> GetImageAsync(CloudProvider provider, string name)
        {
            var image = await db.Images.FindAsync(provider, name).ConfigureAwait(false);

            if (image == null)
            {
                // TODO: Fetch the image details (creation date, architechure, etc)

                image = new ImageInfo {
                    ProviderId = provider.Id,
                    Name = name,
                    Details = new JsonObject {
                        // { "architecture", instance.Architecture }
                    },
                    Created = DateTime.UtcNow
                };

                await db.Images.InsertAsync(image).ConfigureAwait(false);
            }

            return image;
        }

        const int _1GB = 1_073_741_824;

        public async Task<VolumeInfo> GetVolumeAsync(CloudProvider provider, string name, HostInfo host = null)
        {
            var volume = await db.Volumes.FindAsync(provider, name).ConfigureAwait(false);

            if (volume == null)
            {
                var ec2Volume = await ec2.DescribeVolumeAsync(name).ConfigureAwait(false);

                var location = Locations.Get(provider, ec2Volume.AvailabilityZone);

                 volume = new VolumeInfo {
                    Status      = VolumeStatus.Online,
                    ProviderId  = provider.Id,
                    Name        = name,
                    LocationId  = location.Id,
                    HostId      = host?.Id,
                    Size        = (long)ec2Volume.Size * _1GB,
                    Created     = ec2Volume.CreateTime
                };

                await db.Volumes.InsertAsync(volume).ConfigureAwait(false);
            }

            return volume;
        }

        public async Task<NetworkInfo> GetNetworkAsync(CloudProvider provider, string name)
        {
            var network = await db.Networks.FindAsync(provider, name).ConfigureAwait(false);
            
            if (network == null)
            {
                var vpc = await ec2.DescribeVpcAsync(name).ConfigureAwait(false);

                if (vpc == null) throw new Exception($"VPC#{name} not found");
            
                network = new NetworkInfo {
                    ProviderId = provider.Id,
                    Name       = name,
                    Cidr       = vpc.CidrBlock,
                    Created    = DateTime.UtcNow
                };

                await db.Networks.InsertAsync(network).ConfigureAwait(false);
            }

            return network;
        }
    }

    #endregion
}