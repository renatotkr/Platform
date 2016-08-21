namespace Carbon.Platform
{
    public interface ILibraryRegistry
    {
        Library Find(string name, Semver version);

        Library Find(string name, SemverRange range);
    }
}