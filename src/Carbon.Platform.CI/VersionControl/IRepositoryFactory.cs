using Carbon.VersionControl;

namespace Carbon.Platform.VersionControl
{
    public interface IRepositoryFactory
    {
        IRepositoryClient Get(IRepository repository);
    }
}