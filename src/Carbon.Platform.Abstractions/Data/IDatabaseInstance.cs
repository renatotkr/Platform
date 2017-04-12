using Carbon.Platform.Resources;

namespace Carbon.Platform.Data
{
    public interface IDatabaseInstance : IManagedResource
    {
        long DatabaseId { get; }
    }
}