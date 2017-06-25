using System.Threading.Tasks;

using Carbon.Platform.Computing;

namespace Carbon.CI
{
    public interface IDeploymentManager
    {
        Task<DeployResult> DeployAsync(DeployRequest request);

        Task<DeployResult> DeployAsync(IProgram program, IHost host);
    }
}