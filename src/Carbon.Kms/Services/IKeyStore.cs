using System.Collections.Generic;

using System.Threading.Tasks;

using Carbon.Data.Sequences;

namespace Carbon.Kms
{
    public interface IKeyStore
    {
        Task<KeyInfo> GetAsync(Uid id);

        Task<KeyInfo> GetAsync(long ownerId, string name);

        Task<IReadOnlyList<KeyInfo>> ListAsync(long ownerId);

        Task<IReadOnlyList<KeyInfo>> ListAsync(long ownerId, string name);

        Task CreateAsync(KeyInfo key);

        Task DeactivateAsync(Uid key);
        
        Task DeleteAsync(Uid key);
    }
}