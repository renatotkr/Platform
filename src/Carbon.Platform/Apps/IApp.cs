namespace Carbon.Platform.Apps
{
    using Versioning;

    public interface IApp
    {
        long Id { get; }

        SemanticVersion Version { get; }

        string Name { get; }
    }
}