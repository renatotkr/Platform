using System;
using System.Collections.Generic;
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

        public async Task<NetworkInfo> GetAsync(long id)
        {
            return await db.Networks.FindAsync(id) 
                ?? throw ResourceError.NotFound(ResourceTypes.Network, id);
        }

        // 1 | aws:vpc1

        public async Task<NetworkInfo> GetAsync(string name)
        {
            if (long.TryParse(name, out var id))
            {
                return await GetAsync(id);
            }

            var (provider, resourceId) = ResourceName.Parse(name);

            return await FindAsync(provider, resourceId)
                ?? throw ResourceError.NotFound(ManagedResource.Network(provider, name));            
        }

        public Task<IReadOnlyList<NetworkInfo>> ListAsync(long ownerId)
        {
            return db.Networks.QueryAsync(
                And(Eq("ownerId", ownerId), IsNull("deleted")
            ));
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
            Validate.Object(request, nameof(request)); // Validate the request

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