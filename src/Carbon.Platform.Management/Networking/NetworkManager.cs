using System;
using System.Collections.Generic;
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

        private async Task<IReadOnlyList<NetworkInfo>> SyncAsync()
        {
            var vpcs = await ec2Client.DescribeVpcsAsync(new DescribeVpcsRequest());

            var networks = new List<NetworkInfo>();

            foreach (var vpc in vpcs.Vpcs)
            {
                networks.Add(await GetAsync(vpc.VpcId));
            }

            return networks;
        }

        private async Task<NetworkInfo> GetAsync(string vpcId)
        {
            var network = await networkService.FindAsync(Aws, vpcId);;
            
            if (network == null)
            {
                var vpc = await ec2Client.DescribeVpcAsync(vpcId)
                    ?? throw ResourceError.NotFound(Aws, ResourceTypes.Network, vpcId);

                var region = Locations.Get(Aws, ec2Client.Region.Name);

                var registerRequest = new RegisterNetworkRequest(
                    addressBlocks : new[] { vpc.CidrBlock },
                    resource      : ManagedResource.Network(region, vpc.VpcId),
                    ownerId       : 1
                );

                // TODO: Sync the subnets & security groups

                // TODO: Support ipv6 address blocks

                // Register the network with the platform
                network = await networkService.RegisterAsync(registerRequest);;
            }

            return network;
        }

        public async Task<SubnetInfo> GetSubnetAsync(ResourceProvider provider, string subnetId)
        {
            var subnet = await subnetService.FindAsync(provider, subnetId);;

            if (subnet == null)
            {
                var awsSubnet = await ec2Client.DescribeSubnetAsync(subnetId)
                    ?? throw ResourceError.NotFound(Aws, ResourceTypes.Subnet, subnetId);

                var location = Locations.Get(Aws, awsSubnet.AvailabilityZone);

                var network = await networkService.GetAsync(Aws, awsSubnet.VpcId);;
                
                var createRequest = new RegisterSubnetRequest(
                    addressBlocks : new[] { awsSubnet.CidrBlock },
                    networkId     : network.Id,
                    resource      : ManagedResource.Subnet(location, awsSubnet.SubnetId)
                );

                // TODO: Include the ipv6 address blocks

                // Register the subnet with the platform
                subnet = await subnetService.RegisterAsync(createRequest);;
            }

            return subnet;
        }
    }
}