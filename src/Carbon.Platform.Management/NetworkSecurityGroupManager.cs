using System;
using System.Threading.Tasks;

using Carbon.Platform.Resources;
using Carbon.Platform.Networking;
using Carbon.Platform.Services;

using ec2 = Amazon.Ec2;

namespace Carbon.Platform.Management
{
    public class NetworkSecurityGroupManager
    {
        private readonly PlatformDb db;
        private readonly ec2.Ec2Client ec2;
        private readonly NetworkService networkService;

        public NetworkSecurityGroupManager(ec2.Ec2Client ec2)
        {
            this.db  = db  ?? throw new ArgumentNullException(nameof(db));
            this.ec2 = ec2 ?? throw new ArgumentNullException(nameof(ec2));

            networkService = new NetworkService(db);
        }

        private async Task<NetworkSecurityGroup> GetAsync(ec2.NetworkInterfaceSecurityGroup group, NetworkInfo network)
        {
            var nsg = await db.NetworkSecurityGroups.FindAsync(ResourceProvider.Aws, group.GroupId).ConfigureAwait(false);


            if (nsg == null)
            {
                var resource = new ManagedResource(ResourceProvider.Aws, ResourceType.NetworkSecurityGroup, group.GroupId);

                var createRequest = new CreateNetworkSecurityGroupRequest(
                    name: group.GroupName,
                    networkId: network.Id,
                    resource: resource
                );

                nsg = await networkService.CreateNetworkSecurityGroupAsync(createRequest);
            }

            return nsg;
        }
    }
}