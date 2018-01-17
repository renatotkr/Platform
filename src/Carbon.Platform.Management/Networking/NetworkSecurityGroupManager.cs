using System;
using System.Threading.Tasks;

using Carbon.Platform.Resources;

using Amazon.Ec2;

namespace Carbon.Platform.Networking
{
    public class NetworkSecurityGroupManager
    {
        private readonly INetworkSecurityGroupService nsgService;

        public NetworkSecurityGroupManager(INetworkSecurityGroupService nsgService)
        {
            this.nsgService = nsgService ?? throw new ArgumentNullException(nameof(nsgService));
        }

        public async Task<NetworkSecurityGroup> GetAsync(
            NetworkInfo network, 
            NetworkInterfaceSecurityGroup networkSecurityGroup)
        {
            Ensure.NotNull(network, nameof(network));
            Ensure.NotNull(networkSecurityGroup, nameof(networkSecurityGroup));

            var region = Locations.Get(network.LocationId);

            var nsg = await nsgService.FindAsync(ResourceProvider.Aws, networkSecurityGroup.GroupId);;

            if (nsg == null)
            {
                var registerRequest = new RegisterNetworkSecurityGroupRequest(
                    name      : networkSecurityGroup.GroupName,
                    networkId : network.Id,
                    resource  : ManagedResource.NetworkSecurityGroup(region, networkSecurityGroup.GroupId)
                );

                nsg = await nsgService.RegisterAsync(registerRequest);;
            }

            return nsg;
        }
    }
}