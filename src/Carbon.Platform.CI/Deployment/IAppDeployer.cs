using System.Threading.Tasks;
using Carbon.Platform.Apps;
using Carbon.Platform.Computing;
using Carbon.Versioning;

namespace Carbon.Platform.CI
{
    public interface IAppDeployer
    {
        Task<DeployResult> DeployAsync(IApp app, SemanticVersion version, IEnvironment env);

        Task<DeployResult> DeployAsync(IApp app, SemanticVersion version, IHost host);

        Task RestartAsync(IApp app, IHost host);
    }
}