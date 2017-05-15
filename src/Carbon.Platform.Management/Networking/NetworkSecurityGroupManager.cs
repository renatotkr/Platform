using System;
using System.Threading.Tasks;

using Carbon.Platform.Resources;

using Amazon.Ec2;

namespace Carbon.Platform.Networking
{
    public class NetworkSecurityGroupManager
    {
        private readonly INetworkSecurityGroupService nsgService;
        private readonly Ec2Client ec2;

        public NetworkSecurityGroupManager(
            INetworkSecurityGroupService nsgService,
            Ec2Client ec2)
        {
            this.nsgService = nsgService ?? throw new ArgumentNullException(nameof(nsgService));
            this.ec2        = ec2        ?? throw new ArgumentNullException(nameof(ec2));
        }

        private async Task<NetworkSecurityGroup> GetAsync(NetworkInfo network, NetworkInterfaceSecurityGroup group)
        {
            #region Preconditions

            if (network == null)
                throw new ArgumentNullException(nameof(network));

            if (group == null)
                throw new ArgumentNullException(nameof(group));

            #endregion

            var nsg = await nsgService.FindAsync(ResourceProvider.Aws, group.GroupId).ConfigureAwait(false);

            if (nsg == null)
            {
                var resource = new ManagedResource(ResourceProvider.Aws, ResourceType.NetworkSecurityGroup, group.GroupId);

                var registerRequest = new RegisterNetworkSecurityGroupRequest(
                    name      : group.GroupName,
                    networkId : network.Id,
                    resource  : resource
                );

                nsg = await nsgService.RegisterAsync(registerRequest);
            }

            return nsg;
        }
    }
}