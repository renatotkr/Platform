using System.Threading.Tasks;

using Carbon.Platform.Computing;

namespace Carbon.CI
{
    public interface IApplicationManager
    {
        Task<DeployResult> DeployAsync(DeployApplicationRequest request);

        Task<DeployResult> DeployAsync(ProgramRelease release, IHost host);
    }
}