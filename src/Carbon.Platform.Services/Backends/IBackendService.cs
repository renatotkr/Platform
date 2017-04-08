using System.Threading.Tasks;
using Carbon.Platform.Apps;
using Carbon.Platform.Computing;

namespace Carbon.Platform
{
    public interface IBackendService
    {
        Task<IBackend> FindAsync(IAppEnvironment env, ILocation location);

        Task<IBackend> FindAsync(long id);

        Task<BackendInstance[]> GetInstancesAsync(IAppEnvironment env);

        Task<BackendInstance[]> GetInstancesAsync(long backendId);
    }
}