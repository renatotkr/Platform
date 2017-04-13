using System;
using System.Net;
using System.Threading.Tasks;

using Carbon.Net;
using Carbon.Platform.Resources;

using ec2 = Amazon.Ec2;

namespace Carbon.Platform.Networking
{
    public class NetworkService
    {
        private readonly PlatformDb db;
        private readonly ec2.Ec2Client ec2;

        public NetworkService(PlatformDb db, ec2.Ec2Client ec2)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.ec2 = ec2;
        }

        private static readonly ResourceProvider aws = ResourceProvider.Amazon;

        public Task<NetworkInfo> GetNetworkAsync(long id)
        {
            return db.Networks.FindAsync(id);
        }

        public async Task<NetworkInfo> GetNetworkAsync(ResourceProvider provider, string id)
        {
            var network = await db.Networks.FindAsync(aws, id).ConfigureAwait(false);
            
            if (network == null)
            {
                network = await GetEc2NetworkAsync(id).ConfigureAwait(false);

                await db.Networks.InsertAsync(network).ConfigureAwait(false);
            }

            return network;
        }

        public async Task<NetworkInterfaceInfo> GetNetworkInterfaceAsync(ResourceProvider provider, string id)
        {
            var record = await db.NetworkInterfaces.FindAsync(provider, id).ConfigureAwait(false);

            if (record == null)
            {
                var n = await ec2.DescribeNetworkInterfaceAsync(id).ConfigureAwait(false);

                if (n == null) throw new Exception("No ec2:NetworkInterface with id:" + id);

                var host = await db.Hosts.FindAsync(provider, n.Attachment.InstanceId).ConfigureAwait(false);

                var network = await GetNetworkAsync(aws, n.VpcId).ConfigureAwait(false);

                // Ensure all the security groups exist
                if (n.Groups != null)
                {
                    foreach (var group in n.Groups)
                    {
                        await GetNetworkSecurityGroupAsync(group, network);                       
                    }
                }
                
                var nic = await ConfigureEc2NetworkInterfaceAsync(n).ConfigureAwait(false);

                nic.HostId = host?.Id;

                await db.NetworkInterfaces.InsertAsync(nic).ConfigureAwait(false);
            }

            return record;
        }

        public async Task<SubnetInfo> GetSubnetAsync(ResourceProvider provider, string id)
        {
            var record = await db.Subnets.FindAsync(provider, id).ConfigureAwait(false);

            if (record == null)
            {
                record = await GetEc2SubnetAsync(id);

                await db.Subnets.InsertAsync(record).ConfigureAwait(false);
            }

            return record;
        }

        #region EC2 Helpers

        private async Task<NetworkSecurityGroup> GetNetworkSecurityGroupAsync(ec2.Group group, NetworkInfo network)
        {
            var acl = await db.NetworkSecurityGroups.FindAsync(aws, group.GroupId).ConfigureAwait(false);

            if (acl == null)
            {
                var aclId = await db.NetworkSecurityGroups.GetNextScopedIdAsync(network.Id).ConfigureAwait(false);

                acl = new NetworkSecurityGroup(aclId, group.GroupName) {
                    ProviderId = aws.Id,
                    ResourceId = group.GroupId
                };

                try
                {
                    await db.NetworkSecurityGroups.InsertAsync(acl).ConfigureAwait(false);
                }
                catch
                {
                    throw new Exception("Error creating network ACL:" + acl.Id);
                }
            }

            return acl;
        }

        internal async Task<NetworkInterfaceInfo> ConfigureEc2NetworkInterfaceAsync(ec2.NetworkInterface nic)
        {
            #region Preconditions

            if (nic == null)
                throw new ArgumentNullException(nameof(nic));

            #endregion
            
            var networkInterface = new NetworkInterfaceInfo(
                id        : db.Context.GetNextId<NetworkInterfaceInfo>(),
                mac       : MacAddress.Parse(nic.MacAddress),
                addresses : Array.Empty<IPAddress>(), // todo
                resource  : new ManagedResource(aws, ResourceType.NetworkInterface, nic.NetworkInterfaceId)
            );

            // TODO: Create an attachment...

            //  ni.Attachment?.AttachTime ?? DateTime.UtcNow

            if (nic.SubnetId != null)
            {
                var subnet = await GetSubnetAsync(aws, nic.SubnetId).ConfigureAwait(false);

                networkInterface.SubnetId = subnet.Id; 
            }

            return networkInterface;
        }

        private async Task<SubnetInfo> GetEc2SubnetAsync(string id)
        {
            var s = await ec2.DescribeSubnetAsync(id).ConfigureAwait(false);

            if (s == null) throw new Exception($"Subnet#{id} not found");

            var location = Locations.Get(aws, s.AvailabilityZone);

            var network = await GetNetworkAsync(aws, s.VpcId).ConfigureAwait(false);

            var subnetId = await db.Subnets.GetNextScopedIdAsync(network.Id).ConfigureAwait(false);

            return new SubnetInfo(
                id       : subnetId,
                cidr     : s.CidrBlock,
                resource : ManagedResource.NetworkInterface(location, s.SubnetId)
            );
        }

        private async Task<NetworkInfo> GetEc2NetworkAsync(string id)
        {
            var vpc = await ec2.DescribeVpcAsync(id).ConfigureAwait(false);

            if (vpc == null) throw new Exception($"VPC#{id} not found");

            // TODO: Get the location

            var resource = new ManagedResource(aws, ResourceType.Network, vpc.VpcId);

            return new NetworkInfo(
                id             : db.Context.GetNextId<NetworkInfo>(),
                cidr           : vpc.CidrBlock,
                gatewayAddress : null,
                resource       : resource
            );
        }

        #endregion
    }
}