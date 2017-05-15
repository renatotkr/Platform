using System.Threading.Tasks;

namespace Carbon.Platform.Networking
{
    public interface INetworkInterfaceService
    {
        Task<NetworkInterfaceInfo> FindAsync(ResourceProvider provider, string id);

        Task<NetworkInterfaceInfo> GetAsync(long id);

        Task<NetworkInterfaceInfo> GetAsync(string name);

        Task<NetworkInterfaceInfo> RegisterAsync(RegisterNetworkInterfaceRequest request);
    }
}