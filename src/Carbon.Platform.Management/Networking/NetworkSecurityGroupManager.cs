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

        public async Task<NetworkSecurityGroup> GetAsync(NetworkInfo network, NetworkInterfaceSecurityGroup group)
        {
            #region Preconditions

            if (network == null)
                throw new ArgumentNullException(nameof(network));

            if (group == null)
                throw new ArgumentNullException(nameof(group));

            #endregion

            var region = Locations.Get(network.LocationId);

            var nsg = await nsgService.FindAsync(ResourceProvider.Aws, group.GroupId).ConfigureAwait(false);

            if (nsg == null)
            {
                var registerRequest = new RegisterNetworkSecurityGroupRequest(
                    name      : group.GroupName,
                    networkId : network.Id,
                    resource  : ManagedResource.NetworkSecurityGroup(region, group.GroupId)
                );

                nsg = await nsgService.RegisterAsync(registerRequest).ConfigureAwait(false);
            }

            return nsg;
        }
    }
}