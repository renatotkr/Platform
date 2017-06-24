using System.Threading.Tasks;

using Carbon.Data.Protection;

namespace Carbon.Kms
{
    public interface IKeyProvider
    {
        ValueTask<CryptoKey> GetAsync(long keyId);
    }
}