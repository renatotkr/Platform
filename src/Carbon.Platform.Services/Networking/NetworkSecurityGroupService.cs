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

        public async Task<NetworkSecurityGroup> GetAsync(long id)
        {
            return await db.NetworkSecurityGroups.FindAsync(id) 
                ?? throw ResourceError.NotFound(ResourceTypes.NetworkSecurityGroup, id);
        }

        public async Task<NetworkSecurityGroup> FindAsync(ResourceProvider provider, string resourceId)
        {
            Ensure.NotNullOrEmpty(resourceId, nameof(resourceId));

            return await db.NetworkSecurityGroups.FindAsync(provider, resourceId);
        }

        public async Task<NetworkSecurityGroup> RegisterAsync(RegisterNetworkSecurityGroupRequest request)
        {
            Ensure.Object(request, nameof(request)); // Validate the request

            var id = await db.NetworkSecurityGroups.GetNextScopedIdAsync(request.NetworkId);

            var nsg = new NetworkSecurityGroup(
                id       : id,
                name     : request.Name,
                resource : request.Resource
            );

            await db.NetworkSecurityGroups.InsertAsync(nsg);

            return nsg;
        }

        public async Task<bool> DeleteAsync(INetworkSecurityGroup record)
        {
            Ensure.NotNull(record, nameof(record));

            return await db.NetworkSecurityGroups.PatchAsync(record.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;
        }
    }
}