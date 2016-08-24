using System.Threading.Tasks;

namespace Carbon.Platform
{
    using Packaging;

    public interface IRepositoryClient
    {
        Task<Package> DownloadAsync(Revision revision);

        Task<ICommit> GetCommitAsync(Revision revision);

        Task TagAsync(ICommit commit, Semver version);
    }
}