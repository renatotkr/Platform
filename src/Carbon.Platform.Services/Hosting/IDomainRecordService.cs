using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Hosting
{
    public interface IDomainRecordService
    {
        Task<IReadOnlyList<DomainRecord>> QueryAsync(string name, DomainRecordType type);

        Task<DomainRecord> CreateAsync(CreateDomainRecordRequest request);

        Task UpdateAsync(UpdateDomainRecordRequest request);

        Task DeleteAsync(long id);
    }
}