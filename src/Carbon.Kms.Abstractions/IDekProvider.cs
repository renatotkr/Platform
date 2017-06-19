using System.Threading.Tasks;

using Carbon.Data.Protection;

namespace Carbon.Kms
{
    public interface IDekProvider
    {
        ValueTask<DataKey> GetAsync(string keyId);

        ValueTask<DataKey> GetAsync(string keyId, int keyVersion);
    }
}