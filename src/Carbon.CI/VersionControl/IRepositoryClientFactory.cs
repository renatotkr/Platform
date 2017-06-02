using Carbon.VersionControl;

namespace Carbon.Platform.Storage
{
    public interface IRepositoryClientFactory
    {
        IRepositoryClient Get(IRepository repository);
    }
}