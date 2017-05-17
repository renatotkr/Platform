using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Computing
{
    public interface IProgramService
    {
        Task<Program> GetAsync(long id);

        Task<Program> FindAsync(string slug);

        Task<IReadOnlyList<Program>> ListAsync(long ownerId);

        Task<Program> CreateAsync(CreateProgramRequest request);

        Task<EnvironmentInfo> GetEnvironmentAsync(long programId, EnvironmentType type);

        Task<IReadOnlyList<EnvironmentInfo>> GetEnvironmentsAsync(IProgram program);
    }
}