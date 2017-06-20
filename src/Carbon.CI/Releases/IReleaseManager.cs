using System.Threading.Tasks;
using Carbon.Platform.Computing;
using Carbon.Storage;
using Carbon.Versioning;

namespace Carbon.CI
{
    public interface IReleaseManager
    {
        Task<ProgramRelease> CreateAsync(
            Program program, 
            SemanticVersion version, 
            IPackage package, 
            long creatorId, 
            long? keyId = default(long?)
        );

        Task<IPackage> DownloadAsync(ProgramRelease release);
    }
}