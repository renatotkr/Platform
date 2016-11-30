using System.Threading.Tasks;

namespace Carbon.Packaging
{
    using Protection;
    using Versioning;

    public interface IPackageStore
    {
        Task<Package> GetAsync(string name, SemanticVersion version);

        Task<Hash> PutAsync(string name, SemanticVersion version, Package package);
    }
}