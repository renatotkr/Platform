namespace Carbon.Platform.Web
{
    using Versioning;

    public interface IWebLibraryRegistry
    {
        ILibrary Find(string name, SemanticVersion version);

        ILibrary Find(string name, SemanticVersionRange range);
    }
}