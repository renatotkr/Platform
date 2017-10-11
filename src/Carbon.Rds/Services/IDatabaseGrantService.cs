using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Security;

namespace Carbon.Rds.Services
{
    public interface IDatabaseGrantService
    {
        Task<DatabaseGrant> CreateAsync(CreateDatabaseGrantRequest request);

        Task DeleteAsync(IDatabaseGrant grant);

        Task<IReadOnlyList<DatabaseGrant>> ListAsync(IDatabaseInfo database);
        
        Task<IReadOnlyList<DatabaseGrant>> ListAsync(IUser user);

        Task<IReadOnlyList<DatabaseGrant>> ListAsync(IUser user, IDatabaseInfo database);
    }
}