using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public interface IQueueInfo : IManagedResource
    {
        string Name { get; }
    }
}