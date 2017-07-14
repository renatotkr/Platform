using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Rds.Services
{
    public interface IDatabaseService
    {
        Task<IReadOnlyList<DatabaseInfo>> ListAsync(long ownerId);

        Task<DatabaseInfo> RegisterAsync(RegisterDatabaseRequest request);
    }
}