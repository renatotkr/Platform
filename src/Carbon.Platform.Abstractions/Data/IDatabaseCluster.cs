using Carbon.Platform.Resources;

namespace Carbon.Platform.Data
{
    public interface IDatabaseCluster : IManagedResource
    {
        long DatabaseId { get; }

        string Name { get; }
    }
}