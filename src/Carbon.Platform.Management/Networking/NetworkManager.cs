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
        private readonly Ec2Client ec2Client;

        public NetworkManager(
            INetworkService networkService,
            ISubnetService subnetService, 
            Ec2Client ec2Client)
        {
            this.networkService = networkService ?? throw new ArgumentNullException(nameof(networkService));
            this.subnetService  = subnetService  ?? throw new ArgumentNullException(nameof(subnetService));
            this.ec2Client      = ec2Client      ?? throw new ArgumentNullException(nameof(ec2Client));
        }

        private async Task<NetworkInfo> GetAsync(string vpcId)
        {
            var network = await networkService.FindAsync(Aws, vpcId).ConfigureAwait(false);
            
            if (network == null)
            {
                var vpc = await ec2Client.DescribeVpcAsync(vpcId).ConfigureAwait(false)
                    ?? throw new Exception($"aws:network/{vpcId} not found");

                var region = Locations.Get(Aws, ec2Client.Region.Name);

                var registerRequest = new RegisterNetworkAsync(
                    addressBlocks : new[] { vpc.CidrBlock },
                    resource      : ManagedResource.Network(region, vpc.VpcId)
                );

                // TODO: Support ipv6 blocks...

                network = await networkService.RegisterAsync(registerRequest).ConfigureAwait(false);
            }

            return network;
        }

        public async Task<SubnetInfo> GetSubnetAsync(ResourceProvider provider, string subnetId)
        {
            var subnet = await subnetService.FindAsync(provider, subnetId).ConfigureAwait(false);

            if (subnet == null)
            {
                var awsSubnet = await ec2Client.DescribeSubnetAsync(subnetId).ConfigureAwait(false)
                    ?? throw new Exception($"aws:subnet/{subnetId} not found");

                var location = Locations.Get(Aws, awsSubnet.AvailabilityZone);

                var network = await networkService.GetAsync(Aws, awsSubnet.VpcId).ConfigureAwait(false);                

                var createRequest = new RegisterSubnetRequest(
                    addressBlocks : new[] { awsSubnet.CidrBlock },
                    networkId     : network.Id,
                    resource      : ManagedResource.Subnet(location, awsSubnet.SubnetId)
                );

                // TODO: include Ipv6CidrBlocks, if used

                // Register the subnet with the platform
                subnet = await subnetService.RegisterAsync(createRequest).ConfigureAwait(false);
            }

            return subnet;
        }
    }
}