using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Platform.Environments;
using Carbon.Security;

namespace Carbon.CI
{
    public interface IDeploymentManager
    {
        Task<Deployment> GetAsync(long id);

        Task<IReadOnlyList<Deployment>> ListAsync(IEnvironment environment);

        Task<IReadOnlyList<Deployment>> ListAsync(IEnvironment environment, long programId);

        Task<Deployment> DeployAsync(DeployRequest request, ISecurityContext context);
    }
}