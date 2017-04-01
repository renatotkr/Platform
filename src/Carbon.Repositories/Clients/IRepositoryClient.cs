using System.Threading.Tasks;

namespace Carbon.Repositories
{
    using Storage;

    public interface IRepositoryClient
    {
        Task<IPackage> DownloadAsync(Revision revision);

        Task<ICommit> GetCommitAsync(Revision revision);
    }
}