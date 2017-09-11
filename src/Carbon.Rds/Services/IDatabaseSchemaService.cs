using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Rds.Services
{
    public interface IDatabaseSchemaService
    {
        Task<DatabaseSchema> CreateAsync(CreateDatabaseSchemaRequest request);

        Task DeleteAsync(IDatabaseSchema schema);

        Task<IReadOnlyList<DatabaseBackup>> ListAsync(IDatabaseInfo database);
    }
}