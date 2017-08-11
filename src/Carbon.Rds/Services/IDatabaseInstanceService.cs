using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Rds.Services
{
    public interface IDatabaseInstanceService
    {
        Task DeleteAsync(DatabaseInstance instance);

        Task<IReadOnlyList<DatabaseInstance>> ListAsync(IDatabaseInfo database);

        Task<DatabaseInstance> RegisterAsync(RegisterDatabaseInstanceRequest request);
    }
}