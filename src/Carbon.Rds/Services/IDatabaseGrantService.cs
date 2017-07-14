using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Rds.Services
{
    public interface IDatabaseGrantService
    {
        Task<DatabaseGrant> CreateAsync(CreateDatabaseGrantRequest request);

        Task DeleteAsync(IDatabaseGrant grant);

        Task<IReadOnlyList<DatabaseGrant>> ListAsync(IDatabaseInfo database);
    }
}