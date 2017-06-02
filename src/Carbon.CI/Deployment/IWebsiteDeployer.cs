using System.Threading.Tasks;

namespace Carbon.CI
{
    public interface IWebsiteDeployer
    {
        Task<DeployResult> DeployAsync(DeployWebsiteRequest request);
    }
}