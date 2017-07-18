using Carbon.Platform.Storage;
using Carbon.VersionControl;

namespace Carbon.CI
{
    public interface IRepositoryClientFactory
    {
        IRepositoryClient Get(IRepository repository);
    }
}