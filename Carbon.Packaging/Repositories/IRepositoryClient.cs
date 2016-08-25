using System.Threading.Tasks;

namespace Carbon.Packaging
{
    using Storage;

    public interface IRepositoryClient
    {
        Task<Package> DownloadAsync(Revision revision);

        Task<ICommit> GetCommitAsync(Revision revision);

        Task TagAsync(ICommit commit, Semver version);
    }
}