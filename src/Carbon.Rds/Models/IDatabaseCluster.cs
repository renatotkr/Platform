using Carbon.Platform.Resources;

namespace Carbon.Rds
{
    public interface IDatabaseCluster : IResource
    {
        long DatabaseId { get; }

        string Name { get; }
    }
}