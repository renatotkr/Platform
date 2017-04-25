using System.Threading.Tasks;

using Carbon.Platform.Apps;
using Carbon.Platform.Computing;
using Carbon.Platform.Data;

namespace Carbon.Platform
{
    public interface IEnvironmentService
    {
        Task<AppEnvironment> GetAsync(IApp app, string name);

        Task<AppEnvironment> GetAsync(IApp app, EnvironmentName type);

        Task<IHost[]> GetHostsAsync(IEnvironment env);

        Task<IHost[]> GetHostsAsync(IEnvironment env, ILocation location);
    }
}