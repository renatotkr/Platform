using System;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    using static Expression;

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
            #region Preconditions

            if (resourceId == null)
                throw new ArgumentNullException(nameof(resourceId));

            #endregion

            return await db.NetworkSecurityGroups.FindAsync(provider, resourceId);
        }

        public async Task<NetworkSecurityGroup> RegisterAsync(RegisterNetworkSecurityGroupRequest request)
        {
            Validate.Object(request, nameof(request)); // Validate the request

            var id = await db.NetworkSecurityGroups.GetNextScopedIdAsync(request.NetworkId);

            var nsg = new NetworkSecurityGroup(
                id       : id,
                name     : request.Name,
                resource : request.Resource
            );

            await db.NetworkSecurityGroups.InsertAsync(nsg);

            return nsg;
        }

        public async Task<bool> DeleteAsync(INetworkSecurityGroup group)
        {
            #region Preconditions

            if (group == null)
                throw new ArgumentNullException(nameof(group));

            #endregion

            return await db.NetworkSecurityGroups.PatchAsync(group.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;
        }
    }
}