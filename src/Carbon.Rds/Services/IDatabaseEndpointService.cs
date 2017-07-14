using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Rds.Services
{
    public interface IDatabaseEndpointService
    {
        Task<IReadOnlyList<DatabaseEndpoint>> ListAsync(IDatabaseInfo database);
    }
}