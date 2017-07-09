using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public interface IDatabaseInstance : IResource
    {
        long DatabaseId { get; }
    }
}