using Carbon.Data;
using System.Threading.Tasks;

namespace Carbon.Packaging
{
    // Add version? 

    public interface IPackageStore
    {
        Task<Package> GetAsync(string name);

        Task<CryptographicHash> PutAsync(string name, Package package);
    }
}