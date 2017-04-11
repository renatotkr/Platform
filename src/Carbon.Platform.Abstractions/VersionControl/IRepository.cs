namespace Carbon.Platform.VersionControl
{
    public interface IRepository : IManagedResource
    {
        long Id { get; }

        string Name { get; }        // platform

        string FullName { get; }    // carbon/platform

        long OwnerId { get; }
    }
}