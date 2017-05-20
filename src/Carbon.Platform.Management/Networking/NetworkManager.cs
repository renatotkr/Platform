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
                    ?? throw ResourceError.NotFound(Aws, ResourceTypes.Network, vpcId);

                var region = Locations.Get(Aws, ec2Client.Region.Name);

                var registerRequest = new RegisterNetworkAsync(
                    addressBlocks : new[] { vpc.CidrBlock },
                    resource      : ManagedResource.Network(region, vpc.VpcId),
                    ownerId       : 1
                );

                // TODO: Support ipv6 address blocks

                // Register the network with the platform
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
                    ?? throw ResourceError.NotFound(Aws, ResourceTypes.Subnet, subnetId);

                var location = Locations.Get(Aws, awsSubnet.AvailabilityZone);

                var network = await networkService.GetAsync(Aws, awsSubnet.VpcId).ConfigureAwait(false);
                
                var createRequest = new RegisterSubnetRequest(
                    addressBlocks : new[] { awsSubnet.CidrBlock },
                    networkId     : network.Id,
                    resource      : ManagedResource.Subnet(location, awsSubnet.SubnetId)
                );

                // TODO: Include the ipv6 address blocks

                // Register the subnet with the platform
                subnet = await subnetService.RegisterAsync(createRequest).ConfigureAwait(false);
            }

            return subnet;
        }
    }
}