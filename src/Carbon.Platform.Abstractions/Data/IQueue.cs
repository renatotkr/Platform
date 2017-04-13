using Carbon.Platform.Resources;

namespace Carbon.Platform.Data
{
    public interface IQueue : IManagedResource
    {
        string Name { get; }
    }
}