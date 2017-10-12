using System;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;
using Carbon.Platform.Services;

namespace Carbon.Platform.Networking
{
    using static Expression;

    public class NetworkService : INetworkService
    {
        private readonly PlatformDb db;

        public NetworkService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<NetworkInfo> GetAsync(long id)
        {
            return db.Networks.FindAsync(id) 
                ?? throw ResourceError.NotFound(ResourceTypes.Network, id);
        }

        // 1 | aws:vpc1

        public Task<NetworkInfo> GetAsync(string name)
        {
            if (long.TryParse(name, out var id)) return GetAsync(id);
            
            (var provider, var resourceId) = ResourceName.Parse(name);

            return FindAsync(provider, resourceId)
                ?? throw ResourceError.NotFound(provider, ResourceTypes.Network, name);            
        }

        public async Task<NetworkInfo> GetAsync(ResourceProvider provider, string resourceId)
        {
            return await db.Networks.FindAsync(provider, resourceId)
                ?? throw ResourceError.NotFound(provider, ResourceTypes.Network, resourceId);
        }

        public async Task<NetworkInfo> FindAsync(ResourceProvider provider, string resourceId)
        {
            return await db.Networks.FindAsync(provider, resourceId);
        }

        public async Task<NetworkInfo> RegisterAsync(RegisterNetworkRequest request)
        {
            #region Validation

            Validate.Object(request, nameof(request));

            #endregion

            var network = new NetworkInfo(
                id            : await db.Networks.Sequence.NextAsync(),
                addressBlocks : request.AddressBlocks,
                ownerId       : request.OwnerId,
                resource      : request.Resource
            );
            
            await db.Networks.InsertAsync(network);

            return network;
        }

        public async Task<bool> DeleteAsync(INetwork network)
        {
            return await db.Networks.PatchAsync(network.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;
        }
    }
}