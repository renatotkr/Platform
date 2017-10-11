using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Sql;
using Carbon.Security;

namespace Carbon.Rds.Services
{
    public interface IDatabaseGrantService
    {
        Task<DatabaseGrant> CreateAsync(CreateDatabaseGrantRequest request);

        Task<bool> DeleteAsync(IDatabaseGrant grant);

        Task<IReadOnlyList<DatabaseGrant>> ListAsync(IDatabaseInfo database);

        Task<IReadOnlyList<DatabaseGrant>> ListAsync(IDatabaseInfo database, IUser user);

        Task<IReadOnlyList<DatabaseGrant>> ListAsync(IDatabaseInfo database, IUser user, DbObject resource);

        Task<IReadOnlyList<DatabaseGrant>> ListAsync(IUser user);
    }
}