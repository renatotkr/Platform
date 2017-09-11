using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Rds.Services
{
    public interface IDatabaseUserService
    {
        Task<DatabaseUser> CreateAsync(CreateDatabaseUserRequest request);

        Task<bool> ExistsAsync(long databaseId, long userId);

        Task<DatabaseUser> GetAsync(long databaseId, long userId);

        Task DeleteAsync(DatabaseUser user);

        Task<IReadOnlyList<DatabaseUser>> ListAsync(IDatabaseInfo database);
    }
}