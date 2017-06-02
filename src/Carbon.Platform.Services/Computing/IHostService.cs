using System.Threading.Tasks;

namespace Carbon.Platform.Computing
{
    public interface IHostService
    {
        Task<HostInfo> GetAsync(long id);

        Task<HostInfo> GetAsync(string name);

        Task<HostInfo> RegisterAsync(RegisterHostRequest request);
        
        Task<HostInfo[]> ListAsync(ICluster cluster);

        Task<HostInfo[]> ListAsync(IEnvironment environment);

    }
}