using System.Threading.Tasks;

namespace Carbon.Packaging
{
    public interface IPackageRegistry
    {
        long LookupId(string name);

        Task<IPackage> FindAsync(long id, Semver version);

        Task CreateAsync(PackageRelease package);
    }
}

// The registry provides information about packages
// The repository provides the actual package data

// Change to ValueTask when stable