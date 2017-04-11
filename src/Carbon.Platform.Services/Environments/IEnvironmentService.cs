using System.Threading.Tasks;

using Carbon.Platform.Computing;

namespace Carbon.Platform
{
    public interface IEnvironmentService
    {
        Task<AppEnvironment> GetAsync(long appId, string name);

        Task<IHost[]> GetHostsAsync(IEnvironment env);

        Task<IHost[]> GetHostsAsync(IEnvironment env, ILocation location);
    }
}