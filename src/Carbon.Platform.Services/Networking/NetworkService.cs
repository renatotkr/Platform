using System;
using System.Net;
using System.Threading.Tasks;

using Carbon.Platform.Networking;

namespace Carbon.Platform.Services
{
    public class NetworkService : INetworkService
    {
        private readonly PlatformDb db;

        public NetworkService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        private static readonly ResourceProvider aws = ResourceProvider.Aws;

        public Task<NetworkInfo> GetAsync(long id)
        {
            return db.Networks.FindAsync(id) ?? throw new Exception("Network#{id} does not exist");
        }

        public async Task<NetworkInfo> FindAsync(ResourceProvider provider, string id)
        {
            return await db.Networks.FindAsync(aws, id).ConfigureAwait(false);
        }

        public async Task<NetworkInfo> CreateAsync(CreateNetworkRequest request)
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

        public async Task<SubnetInfo> CreateSubnetAsync(CreateSubnetRequest request)
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

        public async Task<NetworkSecurityGroup> CreateNetworkSecurityGroupAsync(CreateNetworkSecurityGroupRequest request)
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

        public async Task<NetworkInterfaceInfo> CreateNetworkInterfaceAsync(CreateNetworkInterfaceRequest request)
        {
            #region Preconditions

            Validate.Object(request, nameof(request));

            #endregion

            var networkInterface = new NetworkInterfaceInfo(
                id        : await db.NetworkInterfaces.GetNextScopedIdAsync(request.NetworkId).ConfigureAwait(false),
                addresses : Array.Empty<IPAddress>(),
                mac       : request.Mac,
                subnetId  : request.SubnetId,
                resource  : request.Resource
            );

            await db.NetworkInterfaces.InsertAsync(networkInterface).ConfigureAwait(false);

            return networkInterface;
        }
    }
}