using System;
using System.Threading.Tasks;

using Carbon.Platform.Resources;
using Carbon.Platform.Services;

namespace Carbon.Platform.Networking
{
    public class SubnetService : ISubnetService
    {
        private readonly PlatformDb db;

        public SubnetService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<SubnetInfo> GetAsync(long id)
        {
            return db.Subnets.FindAsync(id) ?? throw ResourceError.NotFound(ResourceTypes.Subnet, id);
        }
        
        public Task<SubnetInfo> GetAsync(string name)
        {
            if (long.TryParse(name, out var id)) return GetAsync(id);

            (var provider, var resourceId) = ResourceName.Parse(name);

            return FindAsync(provider, resourceId) ?? throw ResourceError.NotFound(provider, ResourceTypes.Network, name);            
        }

        public async Task<SubnetInfo> GetAsync(ResourceProvider provider, string resourceId)
        {
            return await db.Subnets.FindAsync(provider, resourceId).ConfigureAwait(false)
                ?? throw ResourceError.NotFound(provider, ResourceTypes.Subnet, resourceId);
        }

        public async Task<SubnetInfo> FindAsync(ResourceProvider provider, string resourceId)
        {
            return await db.Subnets.FindAsync(provider, resourceId).ConfigureAwait(false);
        }

        public async Task<SubnetInfo> RegisterAsync(RegisterSubnetRequest request)
        {
            #region Preconditions

            Validate.Object(request, nameof(request));

            #endregion

            // Open: Get subnetCount from network?

            var subnet = new SubnetInfo(
                id            : await db.Subnets.GetNextScopedIdAsync(request.NetworkId).ConfigureAwait(false),
                addressBlocks : request.AddressBlocks,
                resource      : request.Resource
            );

            await db.Subnets.InsertAsync(subnet).ConfigureAwait(false);

            return subnet;
        }
    }
}