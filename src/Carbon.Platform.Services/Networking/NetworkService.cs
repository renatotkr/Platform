using System;
using System.Net;
using System.Threading.Tasks;

using Carbon.Platform.Resources;
using Carbon.Platform.Services;

namespace Carbon.Platform.Networking
{
    public class NetworkService : INetworkService
    {
        private readonly PlatformDb db;

        public NetworkService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<NetworkInfo> GetAsync(long id)
        {
            return db.Networks.FindAsync(id) ?? throw ResourceError.NotFound(ResourceType.Network, id);
        }

        // 1 | aws:vpc1

        public Task<NetworkInfo> GetAsync(string name)
        {
            if (long.TryParse(name, out var id))
            {
                return GetAsync(id);
            }
            else
            {
                (var provider, var resourceId) = ResourceName.Parse(name);

                return FindAsync(provider, resourceId) ?? throw ResourceError.NotFound(provider, ResourceType.Network, name);
            }
        }

        public async Task<NetworkInfo> GetAsync(ResourceProvider provider, string resourceId)
        {
            return await db.Networks.FindAsync(provider, resourceId).ConfigureAwait(false)
                ?? throw ResourceError.NotFound(provider, ResourceType.Network, resourceId);
        }

        public async Task<NetworkInfo> FindAsync(ResourceProvider provider, string resourceId)
        {
            return await db.Networks.FindAsync(provider, resourceId).ConfigureAwait(false);
        }

        public async Task<NetworkInfo> RegisterAsync(RegisterNetworkAsync request)
        {
            var network = new NetworkInfo(
                id             : db.Networks.Sequence.Next(),
                cidr           : request.Cidr,
                gatewayAddress : null, 
                resource       : request.Resource
            );
            
            await db.Networks.InsertAsync(network).ConfigureAwait(false);

            return network;
        }

        public async Task<SubnetInfo> FindSubnetAsync(ResourceProvider provider, string resourceId)
        {
            return await db.Subnets.FindAsync(provider, resourceId).ConfigureAwait(false);
        }

        public async Task<SubnetInfo> RegisterSubnetAsync(RegisterSubnetRequest request)
        {
            #region Preconditions

            Validate.Object(request, nameof(request));

            #endregion

            var subnet = new SubnetInfo(
                id       : await db.Subnets.GetNextScopedIdAsync(request.NetworkId).ConfigureAwait(false),
                cidr     : request.Cidr,
                resource : request.Resource
            );

            await db.Subnets.InsertAsync(subnet).ConfigureAwait(false);

            return subnet;
        }

        public async Task<NetworkSecurityGroup> RegisterNetworkSecurityGroupAsync(RegisterNetworkSecurityGroupRequest request)
        {
            #region Preconditions

            Validate.Object(request, nameof(request));

            #endregion

            var nsg = new NetworkSecurityGroup(
                id       : await db.NetworkSecurityGroups.GetNextScopedIdAsync(request.NetworkId).ConfigureAwait(false),
                name     : request.Name,
                resource : request.Resource
            );

            await db.NetworkSecurityGroups.InsertAsync(nsg).ConfigureAwait(false);

            return nsg;
        }
    }
}