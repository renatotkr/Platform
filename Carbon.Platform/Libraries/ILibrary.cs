namespace Carbon.Platform
{
    public interface ILibrary
    {
        long Id { get; }

        string Slug { get; }

        Semver Version { get; }
    }
}