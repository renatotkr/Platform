using System;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;
using Carbon.Platform.Services;

namespace Carbon.Platform.Networking
{
    using static Expression;

    public class SubnetService : ISubnetService
    {
        private readonly PlatformDb db;

        public SubnetService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<SubnetInfo> GetAsync(long id)
        {
            return await db.Subnets.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.Subnet, id);
        }
        
        public async Task<SubnetInfo> GetAsync(string name)
        {
            if (long.TryParse(name, out var id))
            {
                return await GetAsync(id);
            }

            var (provider, resourceId) = ResourceName.Parse(name);

            return await FindAsync(provider, resourceId)
                ?? throw ResourceError.NotFound(ManagedResource.Subnet(provider, name));            
        }

        public async Task<SubnetInfo> GetAsync(ResourceProvider provider, string resourceId)
        {
            return await db.Subnets.FindAsync(provider, resourceId)
                ?? throw ResourceError.NotFound(provider, ResourceTypes.Subnet, resourceId);
        }

        public async Task<SubnetInfo> FindAsync(ResourceProvider provider, string resourceId)
        {
            return await db.Subnets.FindAsync(provider, resourceId);
        }

        public async Task<SubnetInfo> RegisterAsync(RegisterSubnetRequest request)
        {
            Validate.Object(request, nameof(request)); // Validate the request

            // TODO: Get subnetCount from network?

            var subnet = new SubnetInfo(
                id            : await db.Subnets.GetNextScopedIdAsync(request.NetworkId),
                addressBlocks : request.AddressBlocks,
                resource      : request.Resource
            );

            await db.Subnets.InsertAsync(subnet);

            return subnet;
        }

        public async Task<bool> DeleteAsync(ISubnet subnet)
        {
            return await db.Subnets.PatchAsync(subnet.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;
        }
    }
}