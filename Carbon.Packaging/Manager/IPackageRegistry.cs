using System.Threading.Tasks;

namespace Carbon.Packaging
{
    public interface IPackageRegistry
    {
        Task<IPackage> FindAsync(string name, Semver version);

        Task<IPackage> FindAsync(string name, SemverRange range);
    }
}

// Change to ValueTask when stable