using System.Threading.Tasks;

using Carbon.Data.Protection;

namespace Carbon.Kms
{
    public interface IDekProvider
    {
        ValueTask<DataKey> GetAsync(long id);

        ValueTask<DataKey> GetAsync(long id, int version);
    }
}