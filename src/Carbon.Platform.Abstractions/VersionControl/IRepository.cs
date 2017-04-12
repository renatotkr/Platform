using Carbon.Platform.Resources;

namespace Carbon.Platform.VersionControl
{
    public interface IRepository : IManagedResource
    {
        string Name { get; }     // platform

        string FullName { get; } // carbon/platform

        long OwnerId { get; }
    }
}