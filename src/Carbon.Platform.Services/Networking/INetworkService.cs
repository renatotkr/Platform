using System.Threading.Tasks;

namespace Carbon.Platform.Networking
{
    public interface INetworkService
    {
        Task<NetworkInfo> GetAsync(long id);

        Task<NetworkInfo> GetAsync(string name);

        Task<NetworkInfo> GetAsync(ResourceProvider provider, string resourceId);

        Task<NetworkInfo> FindAsync(ResourceProvider provider, string resourceId);

        Task<NetworkInfo> RegisterAsync(RegisterNetworkRequest request);
    }
}