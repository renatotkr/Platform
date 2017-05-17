using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public interface IDatabaseInstance : IManagedResource
    {
        long DatabaseId { get; }
    }
}