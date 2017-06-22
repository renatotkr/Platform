using System.Threading.Tasks;

using Carbon.Data.Protection;

namespace Carbon.Kms
{
    public interface IKeyProvider
    {
        ValueTask<DataKey> GetAsync(long keyId);

        ValueTask<DataKey> GetAsync(long keyId, int keyVersion);
    }
}