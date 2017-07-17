using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Computing
{
    public interface IHostTemplateService
    {
        Task<HostTemplate> CreateAsync(CreateHostTemplateRequest request);

        Task<HostTemplate> GetAsync(long id);

        Task<IReadOnlyList<HostTemplate>> ListAsync();
    }
}