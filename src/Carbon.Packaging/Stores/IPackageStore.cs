using System.Threading.Tasks;

namespace Carbon.Packaging
{
    using Protection;

    public interface IPackageStore
    {
        Task<Package> GetAsync(string name, SemanticVersion version);

        Task<Hash> PutAsync(string name, SemanticVersion version, Package package);
    }
}