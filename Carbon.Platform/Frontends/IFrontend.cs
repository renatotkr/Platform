namespace Carbon.Platform
{
    public interface IFrontend
    {
        long Id { get; }

        Semver Version { get; }

        string Name { get; }
    }
}