using System.Threading.Tasks;

using Carbon.Platform.Apps;
using Carbon.Platform.Computing;

namespace Carbon.Platform
{
    public interface IEnvironmentService
    {
        Task<EnvironmentInfo> GetAsync(IApp app, EnvironmentType type);

        Task<IHost[]> GetHostsAsync(IEnvironment env);

        Task<IHost[]> GetHostsAsync(IEnvironment env, ILocation location);
    }
}