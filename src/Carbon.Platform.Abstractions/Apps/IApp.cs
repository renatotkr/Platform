namespace Carbon.Platform.Apps
{
    using Versioning;

    public interface IApp
    {
        long Id { get; }

        string Name { get; }

        SemanticVersion Version { get; }
    }
}