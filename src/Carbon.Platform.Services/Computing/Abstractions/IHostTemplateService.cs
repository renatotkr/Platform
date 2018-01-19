using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Computing
{
    public interface IHostTemplateService
    {
        Task<HostTemplate> CreateAsync(CreateHostTemplateRequest request);

        Task<HostTemplate> GetAsync(long id);

        Task<HostTemplate> FindAsync(long ownerId, string name);

        Task<IReadOnlyList<HostTemplate>> ListAsync();

        Task<IReadOnlyList<HostTemplate>> ListAsync(long ownerId);

        Task<bool> DeleteAsync(IHostTemplate template);
    }
}