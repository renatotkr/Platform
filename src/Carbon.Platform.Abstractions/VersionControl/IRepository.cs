namespace Carbon.Platform.VersionControl
{
    public interface IRepository : IManagedResource
    {
        long Id { get; }

        string Name { get; }
    }
}