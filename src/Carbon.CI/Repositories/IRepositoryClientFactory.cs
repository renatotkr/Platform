using System.Threading.Tasks;

using Carbon.VersionControl;

namespace Carbon.CI
{
    public interface IRepositoryClientFactory
    {
        Task<IRepositoryClient> GetAsync(IRepository repository);
    }
}