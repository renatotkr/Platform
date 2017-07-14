using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Platform.Storage;

namespace Carbon.Rds.Services
{
    public interface IDatabaseBackupService
    {
        Task CompleteAsync(CompleteDatabaseBackupRequest request);

        Task<IReadOnlyList<DatabaseBackup>> ListAsync(IDatabaseInfo database);

        Task<DatabaseBackup> StartAsync(StartDatabaseBackupRequest request);
    }
}