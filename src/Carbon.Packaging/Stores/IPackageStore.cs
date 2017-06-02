using System.Threading.Tasks;

using Carbon.Storage;

namespace Carbon.Packaging
{
    public interface IPackageStore
    {
        Task<IPackage> GetAsync(string name);

        Task<PutPackageResult> PutAsync(string name, IPackage package);
    }
}