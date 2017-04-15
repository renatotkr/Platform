using Carbon.Platform.Resources;

namespace Carbon.Platform.Data
{
    public interface IQueueInfo : IManagedResource
    {
        string Name { get; }
    }
}