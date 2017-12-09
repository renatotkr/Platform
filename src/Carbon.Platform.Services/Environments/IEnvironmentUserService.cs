using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Environments
{
    public interface IEnvironmentUserService
    {
        Task<EnvironmentUser> CreateAsync(CreateEnvironmentUserRequest request);

        Task DeleteAsync(EnvironmentUser environmentUser);

        Task<EnvironmentUser> GetAsync(long environmentId, long userId);

        Task<IReadOnlyList<EnvironmentUser>> ListAsync(IEnvironment environment);
    }
}