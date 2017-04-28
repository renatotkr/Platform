using System.Threading.Tasks;
using Carbon.Platform.Apps;
using Carbon.Platform.Computing;

namespace Carbon.Platform.CI
{
    public interface IAppDeployer
    {
        Task<DeployResult> DeployAsync(AppRelease release, IEnvironment env, long creatorId);

        Task<DeployResult> DeployAsync(AppRelease release, IHost host);

        Task RestartAsync(IApp app, IHost host);
    }
}