using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Platform.Storage;

namespace Carbon.Rds.Services
{
    public interface IDatabaseMigrationService
    {
        Task<DatabaseMigration> CreateAsync(CreateDatabaseMigrationRequest request);

        Task<IReadOnlyList<DatabaseMigration>> ListAsync(IDatabaseInfo database);
    }
}