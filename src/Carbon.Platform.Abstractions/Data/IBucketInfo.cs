using Carbon.Platform.Resources;

namespace Carbon.Platform.Data
{
    public interface IBucketInfo : IManagedResource
    {
        string Name { get; }
    }
}