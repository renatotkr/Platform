using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public interface IBucketInfo : IResource
    {
        string Name { get; }
    }
}