using System.Threading.Tasks;

namespace Carbon.Packaging
{
    public interface IPackageRepository
    {
        Task<Package> GetAsync(long id, SemanticVersion version);

        Task PublishAsync(Package package);
    }
}