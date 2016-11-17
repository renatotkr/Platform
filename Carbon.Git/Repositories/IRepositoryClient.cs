using System.Threading.Tasks;

namespace Carbon.Git
{
    using Packaging;
    using Repositories;

    public interface IRepositoryClient
    {
        Task<Package> DownloadAsync(Revision revision);

        Task<ICommit> GetCommitAsync(Revision revision);

        Task TagAsync(ICommit commit, SemanticVersion version);
    }
}