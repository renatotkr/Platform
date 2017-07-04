using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Protection;

namespace Carbon.Kms
{
    public interface IDataProtectorProvider
    {
        ValueTask<IDataProtector> GetAsync(string keyId, IEnumerable<KeyValuePair<string, string>> aad = null);

        ValueTask<IDataProtector> GetAsync(long ownerId, string name);
    }
}