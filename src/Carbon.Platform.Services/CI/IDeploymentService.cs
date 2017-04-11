using System.Threading.Tasks;

using Carbon.Platform.Computing;
using Carbon.Platform.Apps;

namespace Carbon.Platform.CI
{
    public interface IDeploymentService
    {
        Task<Deployment> StartAsync(IEnvironment env, IAppRelease release);

        Task CompleteAsync(Deployment deployment, bool succceded);

        Task CreateTargetsAsync(IDeployment deployment, IHost[] hosts);
    }
}