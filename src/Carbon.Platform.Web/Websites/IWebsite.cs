namespace Carbon.Platform.Web
{
    public interface IWebsite
    {
        long Id { get; }

        long OwnerId { get; }

        string Name { get; }

        long RepositoryId { get; }
    }
}