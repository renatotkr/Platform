using System.Threading.Tasks;

using Carbon.Platform;
using Carbon.Platform.Computing;

namespace Carbon.CI
{
    public interface IDeploymentService
    {
        Task<Deployment> StartAsync(
            IEnvironment env, 
            IProgramRelease release, 
            long creatorId
        );

        Task CompleteAsync(Deployment deployment, bool succceded);

        Task CreateTargetsAsync(IDeployment deployment, IHost[] hosts);
    }
}