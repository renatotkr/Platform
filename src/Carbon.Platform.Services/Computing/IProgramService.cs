using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Computing
{
    public interface IProgramService
    {
        Task<ProgramInfo> GetAsync(long id);

        Task<ProgramInfo> FindAsync(string slug);

        Task<IReadOnlyList<ProgramInfo>> ListAsync(long ownerId);

        Task<ProgramInfo> CreateAsync(CreateProgramRequest request);

        Task<EnvironmentInfo> GetEnvironmentAsync(long programId, EnvironmentType type);

        Task<IReadOnlyList<EnvironmentInfo>> GetEnvironmentsAsync(IProgram program);
    }
}