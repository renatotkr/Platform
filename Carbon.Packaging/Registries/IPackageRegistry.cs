using System.Threading.Tasks;

namespace Carbon.Packaging
{
    public interface IPackageRegistry
    {
        long Lookup(string name);

        Task<IPackage> GetAsync(long id, Semver version);

        Task CreateAsync(PackageInfo package);
    }
}

// The registry provides information about packages
// The repository provides the actual package data

// Change to ValueTask when stable