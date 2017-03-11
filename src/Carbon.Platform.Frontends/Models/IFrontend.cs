namespace Carbon.Platform.Frontends
{
    using Versioning;

    public interface IFrontend
    {
        long Id { get; }

        SemanticVersion Version { get; }

        string Name { get; }
    }
}