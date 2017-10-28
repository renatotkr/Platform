using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Security;

namespace Carbon.CI
{
    public interface IRepositoryBranchService
    {
        Task<RepositoryBranch> CreateAsync(CreateBranchRequest request, ISecurityContext context);

        Task<RepositoryBranch> GetAsync(long id);

        Task<RepositoryBranch> GetAsync(long repositoryId, string name);

        Task<IReadOnlyList<RepositoryBranch>> ListAsync(IRepository repository);

        Task<bool> DeleteAsync(IRepositoryBranch branch);
    }
}