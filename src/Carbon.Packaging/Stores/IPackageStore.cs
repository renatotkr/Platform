using System.Threading.Tasks;

using Carbon.Storage;

namespace Carbon.Packaging
{
    public interface IPackageStore
    {
        Task<IPackage> GetAsync(string key, GetPackageOptions? options = null);

        Task<PutPackageResult> PutAsync(
            string key,
            IPackage package,
            PutPackageOptions? options = null
        );
    }
}