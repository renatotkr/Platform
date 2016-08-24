using System.Threading.Tasks;

namespace Carbon.Packaging
{
    public interface IPackageRepository
    {
        Task<Package> GetAsync(long id, Semver version);

        Task PublishAsync(Package package);
    }
}