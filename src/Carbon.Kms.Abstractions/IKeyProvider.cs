using System.Threading.Tasks;

using Carbon.Data.Protection;
using Carbon.Data.Sequences;

namespace Carbon.Kms
{
    public interface IKeyProvider
    {
        ValueTask<CryptoKey> GetAsync(long ownerId, string name);

        ValueTask<CryptoKey> GetAsync(Uid id);
    }
}