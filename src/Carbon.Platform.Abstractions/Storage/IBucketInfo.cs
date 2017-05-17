using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public interface IBucketInfo : IManagedResource
    {
        string Name { get; }
    }
}