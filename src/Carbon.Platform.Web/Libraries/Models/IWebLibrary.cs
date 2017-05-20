namespace Carbon.Platform.Web
{
    using Versioning;

    public interface IWebLibrary
    {
        // TODO: id

        string Name { get; }

        SemanticVersion Version { get; }

        LibraryFile Main { get; }
    }
}