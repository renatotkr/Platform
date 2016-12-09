using System.Threading.Tasks;

namespace Carbon.Packaging
{
    using Protection;
    using Storage;
    using Versioning;

    public interface IPackageStore
    {
        Task<IPackage> GetAsync(long id, SemanticVersion version);

        Task<Hash> PutAsync(long id, SemanticVersion version, IPackage package);
    }
}