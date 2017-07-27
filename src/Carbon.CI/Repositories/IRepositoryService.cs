using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Security;

namespace Carbon.CI
{
    public interface IRepositoryService
    {
        Task<RepositoryInfo> GetAsync(long id);

        Task<RepositoryInfo> GetAsync(long ownerId, string name);

        Task<RepositoryInfo> FindAsync(long ownerId, string name);

        Task<RepositoryInfo> CreateAsync(CreateRepositoryRequest request, ISecurityContext context);

        Task<IReadOnlyList<RepositoryInfo>> ListAsync(long ownerId);
    }
}