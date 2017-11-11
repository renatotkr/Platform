using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Sequences;

namespace Carbon.Kms.Services
{
    public interface IKeyGrantService
    {
        Task<KeyGrant> GetAsync(Uid id);

        Task<IReadOnlyList<KeyGrant>> ListAsync(Uid keyId);

        Task<KeyGrant> CreateAsync(CreateKeyGrantRequest request);

        Task<bool> DeleteAsync(KeyGrant grant);
    }
}