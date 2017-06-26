using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Environments
{
    public interface IEnvironmentProgramService
    {
        Task<IReadOnlyList<EnvironmentProgram>> ListAsync(IEnvironment environment);

        Task<EnvironmentProgram> CreateAsync(CreateEnvironmentProgramRequest request);
    }
}