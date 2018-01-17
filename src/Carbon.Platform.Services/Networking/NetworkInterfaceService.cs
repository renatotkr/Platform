using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Computing;
using Carbon.Platform.Resources;
using Carbon.Platform.Services;

namespace Carbon.Platform.Networking
{
    using static Expression;

    public class NetworkInterfaceService : INetworkInterfaceService
    {
        private readonly PlatformDb db;

        public NetworkInterfaceService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<NetworkInterfaceInfo> GetAsync(long id)
        {
            return await db.NetworkInterfaces.FindAsync(id) 
                ?? throw ResourceError.NotFound(ResourceTypes.NetworkInterface, id);
        }

        public async Task<NetworkInterfaceInfo> GetAsync(string name)
        {
            Ensure.NotNullOrEmpty(name, nameof(name));

            if (long.TryParse(name, out var id))
            {
                return await GetAsync(id);
            }

            var (provider, resourceId) = ResourceName.Parse(name);
            
            return await FindAsync(provider, resourceId) 
                ?? throw ResourceError.NotFound(ManagedResource.NetworkInterface(provider, name));
        }

        public async Task<NetworkInterfaceInfo> FindAsync(ResourceProvider provider, string id)
        {
            return await db.NetworkInterfaces.FindAsync(provider, id);
        }

        public Task<IReadOnlyList<NetworkInterfaceInfo>> ListAsync(IHost host)
        {
            Ensure.NotNull(host, nameof(host));

            return db.NetworkInterfaces.QueryAsync(Eq("hostId", host.Id));
        }

        public async Task<NetworkInterfaceInfo> RegisterAsync(RegisterNetworkInterfaceRequest request)
        {
            Ensure.Object(request, nameof(request)); // Validate the request
            
            var nic = new NetworkInterfaceInfo(
                id               : await db.NetworkInterfaces.GetNextScopedIdAsync(request.NetworkId),
                ipAddresses      : Array.Empty<IPAddress>(),
                macAddress       : request.Mac,
                subnetId         : request.SubnetId,
                securityGroupIds : request.SecurityGroupIds,
                resource         : request.Resource
            );

            await db.NetworkInterfaces.InsertAsync(nic);

            return nic;
        }

        public async Task<bool> DeleteAsync(INetworkInterface networkInterface)
        {
            Ensure.NotNull(networkInterface, nameof(networkInterface));

            return await db.NetworkInterfaces.PatchAsync(networkInterface.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;
        }
    }
}