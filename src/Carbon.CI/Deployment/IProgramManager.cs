using System.Threading.Tasks;

using Carbon.Platform.Computing;

namespace Carbon.CI
{
    public interface IProgramManager
    {
        Task<DeployResult> DeployAsync(DeployRequest request);

        Task<DeployResult> DeployAsync(ProgramRelease release, IHost host);
    }
}