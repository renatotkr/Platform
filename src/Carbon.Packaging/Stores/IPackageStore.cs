using System.Threading.Tasks;

using Carbon.Data.Protection;

namespace Carbon.Packaging
{
    using Storage;
    using Versioning;

    public interface IPackageStore
    {
        Task<IPackage> GetAsync(long id, SemanticVersion version);

        Task<Hash> PutAsync(long id, SemanticVersion version, IPackage package);
    }
}