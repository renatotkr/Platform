namespace Carbon.Platform.Repositories
{
    public interface IRepository
    {
        long Id { get; }

        RepositoryType Type { get; }

        string Name { get; }
        
        int ProviderId { get; }
    }
}