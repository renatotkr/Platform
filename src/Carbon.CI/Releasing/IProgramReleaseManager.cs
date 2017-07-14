using System.Threading.Tasks;
using Carbon.Platform.Computing;
using Carbon.Security;
using Carbon.Storage;

namespace Carbon.CI
{
    public interface IProgramReleaseManager
    {
        Task<ProgramRelease> CreateAsync(PublishProgramRequest request, ISecurityContext context);

        Task<IPackage> DownloadAsync(ProgramRelease release);
    }
}