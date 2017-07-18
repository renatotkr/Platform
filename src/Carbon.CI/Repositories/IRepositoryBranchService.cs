using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.CI
{
    public interface IRepositoryBranchService
    {
        Task<RepositoryBranch> CreateAsync(CreateBranchRequest request);

        Task<RepositoryBranch> GetAsync(long id);

        Task<RepositoryBranch> GetAsync(long repositoryId, string name);

        Task<IReadOnlyList<RepositoryBranch>> ListAsync(long repositoryId);
    }
}