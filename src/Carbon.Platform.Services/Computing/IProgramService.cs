using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Platform.Environments;

namespace Carbon.Platform.Computing
{
    public interface IProgramService
    {
        Task<ProgramInfo> GetAsync(long id);

        Task<ProgramInfo> FindAsync(string slug);

        Task<IReadOnlyList<ProgramInfo>> ListAsync(long ownerId);

        Task<IReadOnlyList<ProgramInfo>> ListAsync(long ownerId, ProgramType type);

        Task<IReadOnlyList<ProgramInfo>> ListAsync(IEnvironment environment);

        Task<IReadOnlyList<ProgramInfo>> ListAsync(IHost host);

        Task<ProgramInfo> CreateAsync(CreateProgramRequest request);

        Task<bool> DeleteAsync(IProgram program);
    }
}