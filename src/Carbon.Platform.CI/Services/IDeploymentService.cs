using System.Threading.Tasks;

using Carbon.Platform.Computing;

namespace Carbon.Platform.CI
{
    public interface IDeploymentService
    {
        Task<Deployment> StartAsync(IEnvironment env, IRelease release, long creatorId);

        Task CompleteAsync(Deployment deployment, bool succceded);

        Task CreateTargetsAsync(IDeployment deployment, IHost[] hosts);
    }
}