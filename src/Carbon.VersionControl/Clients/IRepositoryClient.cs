using System.Threading.Tasks;

namespace Carbon.VersionControl
{
    using Storage;

    public interface IRepositoryClient
    {
        Task<IPackage> DownloadAsync(Revision revision);

        Task<ICommit> GetCommitAsync(Revision revision);
        
        // CreateBranch
        // GetBranch
        // ListBranches
        // GetBlob
        // GetDifferences
    }
}