using Carbon.Data;
using System.Threading.Tasks;

namespace Carbon.Packaging
{
    // Add version? 

    public interface IPackageStore
    {
        Task<Package> GetAsync(string name, Semver version);

        Task<Hash> PutAsync(Package package);
    }
}