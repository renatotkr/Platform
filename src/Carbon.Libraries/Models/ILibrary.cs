namespace Carbon.Libraries
{
    using Versioning;

    public interface ILibrary
    {
        string Name { get; }

        SemanticVersion Version { get; }

        LibraryFile Main { get; }
    }
}