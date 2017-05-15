using System.Threading.Tasks;
using Carbon.Platform.Networking;

namespace Carbon.Platform.Services
{
    public interface INetworkService
    {
        Task<NetworkInfo> CreateAsync(CreateNetworkRequest request);

        Task<NetworkInfo> FindAsync(ResourceProvider provider, string id);

        Task<NetworkInfo> GetAsync(long id);
    }
}