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

        Task CreateBranchAsync(CreateBranchRequest request);

        Task<IRepositoryCommit> GetCommitAsync(long repositoryId, byte[] sha);

        Task<IRepositoryCommit> CreateCommitAsync(CreateCommitRequest request);

        Task<RepositoryBranch> GetBranchAsync(long repositoryId, string name);

        Task<IReadOnlyList<RepositoryBranch>> GetBranchesAsync(long repositoryId);

        Task<IReadOnlyList<RepositoryFile>> GetFilesAsync(long repositoryId, string branchName);
    }
}