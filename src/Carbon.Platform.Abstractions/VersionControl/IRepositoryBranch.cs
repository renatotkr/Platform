namespace Carbon.Platform.VersionControl
{
    public interface IRepositoryBranch
    {
        long RepositoryId { get; }

        string Name { get; }
    }
}