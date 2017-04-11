using System.Threading.Tasks;
using Carbon.Platform.Apps;
using Carbon.Platform.Computing;

namespace Carbon.Platform.CI
{
    public interface IAppDeployer
    {
        Task<DeployResult> DeployAsync(IAppRelease release, IEnvironment env);

        Task<DeployResult> DeployAsync(IAppRelease release, IHost host);

        Task RestartAsync(IApp app, IHost host);
    }
}