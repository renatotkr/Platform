namespace Carbon.Libraries
{
    using Versioning;

    public interface ILibraryRegistry
    {
        ILibrary Find(string name, SemanticVersion version);

        ILibrary Find(string name, SemanticVersionRange range);
    }
}