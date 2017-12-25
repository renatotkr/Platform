using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Platform.Environments;

namespace Carbon.Platform.Networking
{
    public interface IDistributionService
    {
        Task<Distribution> GetAsync(long id);

        Task<Distribution> GetAsync(ResourceProvider provider, string resourceId);

        Task<IReadOnlyList<Distribution>> ListAsync(IEnvironment environment);

        Task<IReadOnlyList<Distribution>> ListAsync(long ownerId);

        Task<Distribution> CreateAsync(CreateDistributionRequest request);

        Task ActivateAsync(Distribution record);

        Task DeactivateAsync(Distribution record);

        Task<bool> DeleteAsync(Distribution record);
    }
}