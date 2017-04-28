using Carbon.Platform.Resources;

namespace Carbon.Platform.Data
{
    public interface IChannelInfo : IManagedResource
    {
        string Name { get; }
    }
}