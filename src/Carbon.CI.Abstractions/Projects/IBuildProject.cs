namespace Carbon.CI
{ 
    public interface IProject 
    {
        long Id { get; }

        string Name { get; }

        long RepositoryId { get; }
    }
}