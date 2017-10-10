using System.Threading.Tasks;
using Carbon.Net.Dns;

namespace Carbon.Platform.Hosting
{
    public interface IDomainService
    {
        Task<Domain> GetAsync(long id);

        Task<Domain> FindAsync(DomainName name);

        Task<Domain> CreateAsync(CreateDomainRequest request);

        Task UpdateAsync(UpdateDomainRequest request);
    }
}