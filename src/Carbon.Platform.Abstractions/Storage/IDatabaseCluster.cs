using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public interface IDatabaseCluster : IManagedResource
    {
        long DatabaseId { get; }

        string Name { get; }
    }
}