using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    public interface IProgramReleaseService
    {
        Task<ProgramRelease> CreateAsync(CreateProgramReleaseRequest request);

        Task<ProgramRelease> GetAsync(long programId, SemanticVersion version);

        Task<bool> ExistsAsync(long programId, SemanticVersion version);

        Task<IReadOnlyList<ProgramRelease>> ListAsync(long programId);
    }
}