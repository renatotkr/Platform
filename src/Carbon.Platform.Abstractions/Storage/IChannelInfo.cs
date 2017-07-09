using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public interface IChannelInfo : IResource
    {
        string Name { get; }
    }
}