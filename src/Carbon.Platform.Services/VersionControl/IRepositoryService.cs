using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Storage
{
    public interface IRepositoryService
    {
        Task<RepositoryInfo> GetAsync(long id);

        Task<RepositoryInfo> GetAsync(long ownerId, string name);

        Task<RepositoryInfo> GetAsync(ResourceProvider provider, string fullName);

        Task<RepositoryInfo> CreateAsync(CreateRepositoryRequest request);

        Task<IReadOnlyList<RepositoryInfo>> ListAsync(long ownerId);

        Task<RepositoryBranch> CreateBranchAsync(CreateBranchRequest request);

        Task<RepositoryBranch> GetBranchAsync(long id);

        Task<RepositoryBranch> GetBranchAsync(long repositoryId, string name);

        Task<IReadOnlyList<RepositoryBranch>> ListBranchesAsync(long repositoryId);
    }
}