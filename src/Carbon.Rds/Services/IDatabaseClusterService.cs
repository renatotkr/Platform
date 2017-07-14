using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Rds.Services
{
    public interface IDatabaseClusterService
    {
        Task<IReadOnlyList<DatabaseCluster>> ListAsync(IDatabaseInfo database);

        Task<DatabaseCluster> RegisterAsync(RegisterDatabaseClusterRequest request);
    }
}