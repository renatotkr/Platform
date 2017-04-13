using Carbon.Platform.Resources;

namespace Carbon.Platform.Data
{
    public interface IBucket : IManagedResource
    {
        string Name { get; }
    }
}