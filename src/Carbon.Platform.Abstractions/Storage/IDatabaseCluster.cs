using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public interface IDatabaseCluster : IResource
    {
        long DatabaseId { get; }

        string Name { get; }
    }
}