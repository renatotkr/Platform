using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Data.Protection;

namespace Carbon.Kms
{
    public interface IKeyManager
    {
        Task<IKeyInfo> PutAsync(
            byte[] plaintext,
            KeyType type,
            IEnumerable<KeyValuePair<string, string>> context
        );

        Task<IKeyInfo> GenerateAsync(
            IEnumerable<KeyValuePair<string, string>> context
        );

        Task DeactivateAsync(long keyId); // Reason...

        Task DeleteAsync(long keyId);
    }
}

// Generation
// Storage
// Distribution
// Use
// Destruction