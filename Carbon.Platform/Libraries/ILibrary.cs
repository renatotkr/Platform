namespace Carbon.Platform
{
    public interface ILibrary
    {
        long Id { get; }

        Semver Version { get; }
    }
}