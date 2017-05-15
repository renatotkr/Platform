using System;
using System.Threading.Tasks;

using Carbon.Platform.Resources;

using Amazon.Ec2;

namespace Carbon.Platform.Networking
{
    using static ResourceProvider;

    public class NetworkManager
    {
        private readonly INetworkService networkService;
        private readonly ISubnetService subnetService;
        private readonly Ec2Client ec2;

        public NetworkManager(
            INetworkService networkService,
            ISubnetService subnetService, 
            Ec2Client ec2)
        {
            this.networkService = networkService ?? throw new ArgumentNullException(nameof(networkService));
            this.subnetService  = subnetService ?? throw new ArgumentNullException(nameof(subnetService));
            this.ec2            = ec2 ?? throw new ArgumentNullException(nameof(ec2));
        }

        private async Task<NetworkInfo> GetAsync(string vpcId)
        {
            var network = await networkService.FindAsync(Aws, vpcId).ConfigureAwait(false);
            
            if (network == null)
            {
                var vpc = await ec2.DescribeVpcAsync(vpcId).ConfigureAwait(false)
                    ?? throw new Exception($"aws:network/{vpcId} not found");

                var region = Locations.Get(Aws, ec2.Region.Name);

                var registerRequest = new RegisterNetworkAsync(
                    cidr: vpc.CidrBlock,
                    resource: ManagedResource.Network(region, vpc.VpcId)
                );

                network = await networkService.RegisterAsync(registerRequest).ConfigureAwait(false);
            }

            return network;
        }

        public async Task<SubnetInfo> GetSubnetAsync(ResourceProvider provider, string subnetId)
        {
            var subnet = await subnetService.FindAsync(provider, subnetId).ConfigureAwait(false);

            if (subnet == null)
            {
                var awsSubnet = await ec2.DescribeSubnetAsync(subnetId).ConfigureAwait(false)
                    ?? throw new Exception($"aws:subnet/{subnetId} not found");

                var location = Locations.Get(Aws, awsSubnet.AvailabilityZone);

                var network = await networkService.GetAsync(Aws, awsSubnet.VpcId).ConfigureAwait(false);                

                var createRequest = new RegisterSubnetRequest(
                    cidr        : awsSubnet.CidrBlock,
                    networkId   : network.Id,
                    resource    : ManagedResource.Subnet(location, awsSubnet.SubnetId)
                );

                // Register the subnet with the platform
                subnet = await subnetService.RegisterAsync(createRequest).ConfigureAwait(false);
            }

            return subnet;
        }
    }
}