using System.Threading.Tasks;

namespace Carbon.Platform.Networking
{
    public interface INetworkInterfaceService
    {
        Task<NetworkInterfaceInfo> FindAsync(ResourceProvider provider, string resourceId);

        Task<NetworkInterfaceInfo> GetAsync(long id);

        Task<NetworkInterfaceInfo> RegisterAsync(RegisterNetworkInterfaceRequest request);
    }
}