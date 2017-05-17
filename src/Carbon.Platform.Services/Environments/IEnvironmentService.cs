using System.Threading.Tasks;

using Carbon.Platform.Computing;

namespace Carbon.Platform
{
    public interface IEnvironmentService
    {
        Task<EnvironmentInfo> GetAsync(long id);

        Task<EnvironmentInfo> GetAsync(IProgram program, EnvironmentType type);

        Task<IHost[]> GetHostsAsync(IEnvironment env);

        Task<IHost[]> GetHostsAsync(IEnvironment env, ILocation location);
    }
}