using System.Threading.Tasks;

namespace Carbon.Packaging
{
    public interface IPackageRepository // Registry?
    {
        Task<Package> GetAsync(string name, Semver version);

        Task Publish(Package package);
    }
}