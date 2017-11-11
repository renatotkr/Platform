using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.CI
{
    public interface IRepositoryCommitService
    {
        Task<RepositoryCommit> CreateAsync(CreateCommitRequest request);

        Task<RepositoryCommit> GetAsync(long id);

        Task<RepositoryCommit> FindAsync(long repositoryId, byte[] sha1);

        Task<IReadOnlyList<RepositoryCommit>> ListAsync(IRepository repository);
    }
}