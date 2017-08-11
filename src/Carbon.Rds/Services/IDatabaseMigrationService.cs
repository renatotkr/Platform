using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Rds.Services
{
    public interface IDatabaseMigrationService
    {
        Task<DatabaseMigration> CreateAsync(CreateDatabaseMigrationRequest request);

        Task<IReadOnlyList<DatabaseMigration>> ListAsync(IDatabaseInfo database);
    }
}