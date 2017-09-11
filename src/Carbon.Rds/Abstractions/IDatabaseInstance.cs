using Carbon.Platform.Resources;

namespace Carbon.Rds
{
    public interface IDatabaseInstance : IResource
    {
        long DatabaseId { get; }

        long HostId { get; }
    }
}