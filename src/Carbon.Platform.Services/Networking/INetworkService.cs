using System.Threading.Tasks;

namespace Carbon.Platform.Networking
{
    public interface INetworkService
    {
        Task<NetworkInfo> FindAsync(ResourceProvider provider, string id);

        Task<NetworkInfo> GetAsync(long id);

        Task<NetworkInfo> GetAsync(string name);

        Task<NetworkInfo> RegisterAsync(RegisterNetworkAsync request);
    }
}