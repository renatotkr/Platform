namespace Carbon.Platform.Web
{
    using Versioning;

    public interface IWebLibraryRegistry
    {
        IWebLibrary Find(string name, SemanticVersion version);

        IWebLibrary Find(string name, SemanticVersionRange range);
    }
}