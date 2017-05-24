using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public interface IRepositoryBranch : IResource
    {
        long RepositoryId { get; }

        string Name { get; }
    }
}