using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Environments
{
    public interface IEnvironmentService
    {
        Task<IReadOnlyList<EnvironmentInfo>> ListAsync(long ownerId);

        Task<int> CountAsync(long ownerId);

        Task<EnvironmentInfo> GetAsync(long id);

        Task<EnvironmentInfo> GetAsync(long ownerId, string name);

        Task<EnvironmentInfo> GetAsync(string slug);
        
        Task<EnvironmentInfo> CreateAsync(CreateEnvironmentRequest request);

        Task<bool> ExistsAsync(long ownerId, string name);

        Task<bool> DeleteAsync(IEnvironment environment);
    }
}