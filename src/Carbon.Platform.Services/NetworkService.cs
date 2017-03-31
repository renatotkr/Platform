using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;

using ec2 = Amazon.Ec2;

using Dapper;

namespace Carbon.Platform.Services
{
    using Networking;
    using static Expression;

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

        public async Task<NetworkInfo> GetNetworkAsync(ResourceProvider provider, string id)
        {
            var network = await db.Networks.FindAsync(aws, id).ConfigureAwait(false);
            
            if (network == null)
            {
                network = await GetEc2NetworkAsync(id).ConfigureAwait(false);

                network.Id = GetNextId<NetworkInfo>();

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

                if (n == null)
                {
                    throw new Exception("No ec2:NetworkInterface with id:" + id);
                }

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
                
                var ni = await ConfigureEc2NetworkInterfaceAsync(n).ConfigureAwait(false);

                ni.Id        = GetNextId<NetworkInterfaceInfo>();
                ni.NetworkId = network.Id;
                ni.HostId    = host?.Id;

                await db.NetworkInterfaces.InsertAsync(ni).ConfigureAwait(false);
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

        private async Task<NetworkAcl> GetNetworkSecurityGroupAsync(ec2.Group group, NetworkInfo network)
        {
            var acl = await db.NetworkAcls.FindAsync(aws, group.GroupId).ConfigureAwait(false);

            if (acl == null)
            {
                var aclId = await GetNextScopedIdAsync(db.NetworkAcls, network.Id).ConfigureAwait(false);

                acl = new NetworkAcl {
                    Id         = aclId,
                    Name       = group.GroupName,
                    ProviderId = aws.Id,
                    ResourceId = group.GroupId
                };

                try
                {
                    await db.NetworkAcls.InsertAsync(acl).ConfigureAwait(false);
                }
                catch
                {
                    throw new Exception("Error creating network ACL:" + acl.Id);
                }
            }

            return acl;
        }

        internal async Task<NetworkInterfaceInfo> ConfigureEc2NetworkInterfaceAsync(ec2.NetworkInterface ni)
        {
            #region Preconditions

            if (ni == null) throw new ArgumentNullException(nameof(ni));

            #endregion

            var networkInterface = new NetworkInterfaceInfo {
                ProviderId = aws.Id,
                ResourceId = ni.NetworkInterfaceId,
                MacAddress = ni.MacAddress,
                Addresses = new List<System.Net.IPAddress>(),
                Created    = ni.Attachment?.AttachTime ?? DateTime.UtcNow
            };
            
            // TODO: addresses

            if (ni.SubnetId != null)
            {
                var subnet = await GetSubnetAsync(aws, ni.SubnetId).ConfigureAwait(false);

                networkInterface.SubnetId = subnet.Id; 
            }

            return networkInterface;
        }

        private async Task<SubnetInfo> GetEc2SubnetAsync(string id)
        {
            var s = await ec2.DescribeSubnetAsync(id).ConfigureAwait(false);

            if (s == null)
            {
                throw new Exception($"Subnet#{id} not found");
            }

            var location = Locations.Get(aws, s.AvailabilityZone);

            var network = await GetNetworkAsync(aws, s.VpcId).ConfigureAwait(false);

            var subnetId = await GetNextScopedIdAsync(db.Subnets, network.Id).ConfigureAwait(false);

            return new SubnetInfo {
                Id         = subnetId, 
                ProviderId = aws.Id,
                ResourceId = id,
                LocationId = location.Id,
                Cidr       = s.CidrBlock
            };              
        }

        private async Task<NetworkInfo> GetEc2NetworkAsync(string id)
        {
            var vpc = await ec2.DescribeVpcAsync(id).ConfigureAwait(false);

            if (vpc == null)
            {
                throw new Exception($"VPC#{id} not found");
            }

            return new NetworkInfo {
                ProviderId = aws.Id,
                ResourceId = vpc.VpcId,
                Cidr       = vpc.CidrBlock
            };          
        }

        #endregion

        private long GetNextId<T>()
        {
            var dataset = DatasetInfo.Get<T>();

            using (var connection = db.Context.GetConnection())
            {
                var id = connection.ExecuteScalar<long>($"SELECT `id` FROM `{dataset.Name}` ORDER BY `id` DESC LIMIT 1");

                return id + 1;
            }
        }

        private static async Task<long> GetNextScopedIdAsync<T>(Dataset<T, long> dataset, long scopeId)
        {
            var range = ScopedId.GetRange(scopeId);

            var count = await dataset.CountAsync(Between("id", range.Start, range.End)).ConfigureAwait(false);

            return ScopedId.Create(scopeId, count);
        }
    }
}