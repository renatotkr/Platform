using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Platform.Computing;

namespace Carbon.Platform.Networking
{
    public interface INetworkInterfaceService
    {
        Task<NetworkInterfaceInfo> FindAsync(ResourceProvider provider, string resourceId);

        Task<NetworkInterfaceInfo> GetAsync(long id);

        Task<IReadOnlyList<NetworkInterfaceInfo>> ListAsync(IHost host);

        Task<NetworkInterfaceInfo> RegisterAsync(RegisterNetworkInterfaceRequest request);

        Task<bool> DeleteAsync(INetworkInterface networkInterface);
    }
}