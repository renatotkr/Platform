using System.Threading.Tasks;
using Carbon.Platform.Web;
using Carbon.Versioning;

namespace Carbon.Platform.CI
{
    public interface IWebsiteDeployer
    {
        Task<DeployResult> DeployAsync(IWebsite website, SemanticVersion version, IEnvironment env);
    }
}