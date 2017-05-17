using System.Threading.Tasks;

namespace Carbon.Platform.CI
{
    public interface IWebsiteDeployer
    {
        Task<DeployResult> DeployAsync(DeployWebsiteRequest request);
    }
}