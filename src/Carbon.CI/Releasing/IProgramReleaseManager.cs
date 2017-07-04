using System.Threading.Tasks;
using Carbon.Data.Sequences;
using Carbon.Platform.Computing;
using Carbon.Security;
using Carbon.Storage;
using Carbon.Versioning;

namespace Carbon.CI
{
    public interface IProgramReleaseManager
    {
        Task<ProgramRelease> CreateAsync(
            ProgramInfo program,
            SemanticVersion version,
            IPackage package,
            ISecurityContext context,
            Uid? keyId = null);

        Task<IPackage> DownloadAsync(ProgramRelease release);
    }
}