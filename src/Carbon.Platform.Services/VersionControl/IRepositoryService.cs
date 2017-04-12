using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Platform.Resources;

namespace Carbon.Platform.VersionControl
{
    public interface IRepositoryService
    {
        Task<RepositoryInfo> GetAsync(long id);

        Task<RepositoryInfo> GetAsync(long ownerId, string name);

        Task<RepositoryInfo> CreateAsync(string name, long ownerId, ManagedResource resource);

        Task CreateBranchAsync(CreateBranchRequest request);

        Task<IRepositoryCommit> CreateCommitAsync(CreateCommitRequest request);

        Task<RepositoryBranch> GetBranchAsync(long repositoryId, string name);

        Task<IReadOnlyList<RepositoryBranch>> GetBranchesAsync(long repositoryId);

        Task<IReadOnlyList<RepositoryFile>> GetFilesAsync(long repositoryId, string branchName);
    }
}