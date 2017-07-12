using System.Threading.Tasks;

using Carbon.Platform.Computing;
using Carbon.Security;

namespace Carbon.CI
{
    public interface IDeploymentManager
    {
        Task<DeployResult> DeployAsync(DeployRequest request, ISecurityContext context);

        Task<DeployResult> DeployAsync(IProgram program, IHost host);
    }
}