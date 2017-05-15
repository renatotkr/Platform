using System;
using System.Threading.Tasks;

using Carbon.Platform.Resources;
using Carbon.Platform.Services;

using ec2 = Amazon.Ec2;

namespace Carbon.Platform.Networking
{
    public class NetworkManager
    {
        private readonly PlatformDb db;
        private readonly ec2.Ec2Client ec2;
        private readonly NetworkService networkService;

        public NetworkManager(PlatformDb db, ec2.Ec2Client ec2)
        {
            this.db             = db ?? throw new ArgumentNullException(nameof(db));
            this.ec2            = ec2 ?? throw new ArgumentNullException(nameof(ec2));
            this.networkService = new NetworkService(db);
        }

        private async Task<NetworkInfo> GetAsync(string vpcId)
        {
            var vpc = await ec2.DescribeVpcAsync(vpcId).ConfigureAwait(false)
                ?? throw new Exception($"aws:network/{vpcId} not found");

            var region = Locations.Get(ResourceProvider.Aws, ec2.Region.Name);

            var createRequest = new CreateNetworkRequest(
                cidr     : vpc.CidrBlock,
                resource : ManagedResource.Network(region, vpc.VpcId)
            );

            return await networkService.CreateAsync(createRequest).ConfigureAwait(false);
        }

        public async Task<SubnetInfo> GetSubnetAsync(ResourceProvider provider, string subnetId)
        {
            var subnet = await db.Subnets.FindAsync(provider, subnetId).ConfigureAwait(false);

            if (subnet == null)
            {
                var awsSubnet = await ec2.DescribeSubnetAsync(subnetId).ConfigureAwait(false)
                    ?? throw new Exception($"aws:subnet/{subnetId} not found");

                var location = Locations.Get(ResourceProvider.Aws, awsSubnet.AvailabilityZone);

                var network = await db.Networks.FindAsync(ResourceProvider.Aws, awsSubnet.VpcId).ConfigureAwait(false);

                var createRequest = new CreateSubnetRequest(
                    cidr        : awsSubnet.CidrBlock,
                    networkId   : network.Id, 
                    resource    : ManagedResource.Subnet(location, awsSubnet.SubnetId)
                );

                // Register the subnet with the platform
                subnet = await networkService.CreateSubnetAsync(createRequest);
            }

            return subnet;
        }
    }
}