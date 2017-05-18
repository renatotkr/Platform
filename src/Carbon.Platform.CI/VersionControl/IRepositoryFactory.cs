using Carbon.VersionControl;

namespace Carbon.Platform.Storage
{
    public interface IRepositoryFactory
    {
        IRepositoryClient Get(IRepository repository);
    }
}