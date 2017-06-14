using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Storage
{
    public interface IRepositoryFileService
    {
        Task DeleteAsync(DeleteFileRequest request);

        Task<RepositoryFile> FindAsync(long branchId, string path);

        Task<IReadOnlyList<RepositoryFile>> ListAsync(long branchId);

        Task<IReadOnlyList<RepositoryFile>> ListChangedSince(long branchId, DateTime modified);
        
        Task<RepositoryFile> PutAsync(CreateFileRequest request);
    }
}