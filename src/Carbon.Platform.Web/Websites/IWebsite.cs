namespace Carbon.Platform.Web
{
    public interface IWebsite
    {
        long Id { get; }

        string Name { get; }

        long RepositoryId { get; }
    }
}