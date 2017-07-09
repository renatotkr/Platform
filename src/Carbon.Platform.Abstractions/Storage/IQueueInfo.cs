using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public interface IQueueInfo : IResource
    {
        string Name { get; }
    }
}