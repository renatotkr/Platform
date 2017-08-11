using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Rds.Services
{
    public interface IDatabaseUserService
    {
        Task DeleteAsync(DatabaseUser user);

        Task<bool> ExistsAsync(long databaseId, long userId);

        Task<IReadOnlyList<DatabaseUser>> ListAsync(IDatabaseInfo database);

        Task<DatabaseUser> CreateAsync(CreateDatabaseUserRequest request);
    }
}