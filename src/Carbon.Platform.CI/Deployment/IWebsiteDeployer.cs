using System.Threading.Tasks;

using Carbon.Platform.Web;

namespace Carbon.Platform.CI
{
    public interface IWebsiteDeployer
    {
        Task<DeployResult> DeployAsync(WebsiteRelease release, IEnvironment env, long deployerId);
    }
}