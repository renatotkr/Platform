using Carbon.Platform.Resources;

namespace Carbon.Platform.Data
{
    public interface ITopicInfo : IManagedResource
    {
        string Name { get; }
    }
}