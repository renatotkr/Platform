using System.Threading.Tasks;

namespace Carbon.Packaging
{
    public interface IPackageStore
    {
        Task<Package> GetAsync(string name);

        Task<Hash> PutAsync(string name, Package package);
    }
}
