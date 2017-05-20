using System;
using System.Net;
using System.Threading.Tasks;

using Carbon.Platform.Resources;
using Carbon.Platform.Services;

namespace Carbon.Platform.Networking
{
    public class NetworkInterfaceService : INetworkInterfaceService
    {
        private readonly PlatformDb db;

        public NetworkInterfaceService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<NetworkInterfaceInfo> GetAsync(long id)
        {
            return db.NetworkInterfaces.FindAsync(id) ?? throw ResourceError.NotFound(ResourceTypes.NetworkInterface, id);
        }

        public Task<NetworkInterfaceInfo> GetAsync(string name)
        {
            if (long.TryParse(name, out var id)) return GetAsync(id);
 
            (var provider, var resourceId) = ResourceName.Parse(name);

            return FindAsync(provider, resourceId) ?? throw ResourceError.NotFound(provider, ResourceTypes.NetworkInterface, name);
        }

        public async Task<NetworkInterfaceInfo> FindAsync(ResourceProvider provider, string id)
        {
            return await db.NetworkInterfaces.FindAsync(provider, id).ConfigureAwait(false);
        }

        public async Task<NetworkInterfaceInfo> RegisterAsync(RegisterNetworkInterfaceRequest request)
        {
            #region Preconditions

            Validate.Object(request, nameof(request));

            #endregion

            var nic = new NetworkInterfaceInfo(
                id               : await db.NetworkInterfaces.GetNextScopedIdAsync(request.NetworkId).ConfigureAwait(false),
                ipAddresses      : Array.Empty<IPAddress>(),
                macAddress       : request.Mac,
                subnetId         : request.SubnetId,
                securityGroupIds : request.SecurityGroupIds,
                resource         : request.Resource
            );

            await db.NetworkInterfaces.InsertAsync(nic).ConfigureAwait(false);

            return nic;
        }
    }
}