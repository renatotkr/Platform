using Carbon.Versioning;

namespace Carbon.Platform.Web
{
    public interface IWebsite
    {
        long Id { get; }

        string Name { get; }

        SemanticVersion Version { get; }
    }
}