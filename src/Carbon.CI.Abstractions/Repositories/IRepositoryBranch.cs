namespace Carbon.CI
{ 
    public interface IRepositoryBranch 
    {
        long Id { get; }

        long RepositoryId { get; }

        string Name { get; }
    }
}