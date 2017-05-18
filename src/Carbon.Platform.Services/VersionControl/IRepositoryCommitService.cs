using System.Threading.Tasks;

namespace Carbon.Platform.Storage
{
    public interface IRepositoryCommitService
    {
        Task<RepositoryCommit> CreateAsync(CreateCommitRequest request);

        Task<RepositoryCommit> FindAsync(long repositoryId, byte[] sha1);

        Task<RepositoryCommit> GetAsync(long id);
    }
}