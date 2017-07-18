using System;
using System.Threading.Tasks;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public class NetworkSecurityGroupService : INetworkSecurityGroupService
    {
        private readonly PlatformDb db;

        public NetworkSecurityGroupService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<NetworkSecurityGroup> GetAsync(long id)
        {
            return db.NetworkSecurityGroups.FindAsync(id) 
                ?? throw ResourceError.NotFound(ResourceTypes.NetworkSecurityGroup, id);
        }

        public async Task<NetworkSecurityGroup> FindAsync(ResourceProvider provider, string resourceId)
        {
            return await db.NetworkSecurityGroups.FindAsync(provider, resourceId);
        }

        public async Task<NetworkSecurityGroup> RegisterAsync(RegisterNetworkSecurityGroupRequest request)
        {
            #region Preconditions

            Validate.Object(request, nameof(request));

            #endregion

            var id = await db.NetworkSecurityGroups.GetNextScopedIdAsync(request.NetworkId);

            var nsg = new NetworkSecurityGroup(
                id       : id,
                name     : request.Name,
                resource : request.Resource
            );

            await db.NetworkSecurityGroups.InsertAsync(nsg);

            return nsg;
        }
    }
}