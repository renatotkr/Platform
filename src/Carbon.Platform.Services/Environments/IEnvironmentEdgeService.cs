using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Environments
{
    public interface IEnvironmentEdgeService
    {
        Task<EnvironmentEdge> GetAsync(long environmentId, int locationId);

        Task<IReadOnlyList<EnvironmentEdge>> ListAsync(IEnvironment environment);

        Task<EnvironmentEdge> CreateAsync(CreateEnvironmentEdgeRequest request);

        Task ActivateAsync(EnvironmentEdge record);

        Task DeactivateAsync(EnvironmentEdge record);

        Task DeleteAsync(EnvironmentEdge record);
    }
}