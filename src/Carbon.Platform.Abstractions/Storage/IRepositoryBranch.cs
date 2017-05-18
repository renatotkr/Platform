namespace Carbon.Platform.Storage
{
    public interface IRepositoryBranch
    {
        long RepositoryId { get; }

        string Name { get; }
    }
}