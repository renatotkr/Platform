using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Kms
{
    public interface IDekManager
    {
        Task<IKeyInfo> CreateAsync(
            IEnumerable<KeyValuePair<string, string>> context
        );

        Task DeleteAsync(long dekId);
    }
}

// Generation
// Storage
// Distribution
// Use
// Destruction