using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.CI
{
    public interface IRepositoryService
    {
        Task<RepositoryInfo> GetAsync(long id);

        Task<RepositoryInfo> GetAsync(long ownerId, string name);

        Task<RepositoryInfo> FindAsync(long ownerId, string name);

        Task<RepositoryInfo> CreateAsync(CreateRepositoryRequest request);

        Task<IReadOnlyList<RepositoryInfo>> ListAsync(long ownerId);
    }
}