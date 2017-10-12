using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Net.Dns;

namespace Carbon.Platform.Hosting
{
    public interface IDomainRecordService
    {
        Task<DomainRecord> GetAsync(long id);

        Task<IReadOnlyList<DomainRecord>> ListAsync(IDomain domain);

        Task<IReadOnlyList<DomainRecord>> QueryAsync(/*in*/ DomainName name, DnsRecordType type);

        Task<DomainRecord> CreateAsync(CreateDomainRecordRequest request);

        Task UpdateAsync(UpdateDomainRecordRequest request);

        Task DeleteAsync(long id);
    }
}