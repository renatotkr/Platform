using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public interface IChannelInfo : IManagedResource
    {
        string Name { get; }
    }
}