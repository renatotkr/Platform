namespace Carbon.Platform.Web
{
    using Versioning;

    public interface IWebLibrary
    {
        string Name { get; }

        SemanticVersion Version { get; }

        LibraryFile Main { get; }
    }
}