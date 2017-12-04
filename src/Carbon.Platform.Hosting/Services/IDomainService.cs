using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Net.Dns;
using Carbon.Platform.Environments;

namespace Carbon.Platform.Hosting
{
    public interface IDomainService
    {
        Task<Domain> GetAsync(long id);

        Task<Domain> GetAsync(DomainName name);

        Task<Domain> FindAsync(DomainName name);

        Task<Domain> CreateAsync(CreateDomainRequest request);

        Task BindAsync(IDomain domain, IEnvironment environment);

        Task UnbindAsync(IDomain domain);

        Task<IReadOnlyList<Domain>> ListAsync(IEnvironment environment);

        // Task<IReadOnlyList<Domain>> ListAsync(IAgent owner);
    }
}