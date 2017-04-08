namespace Carbon.Platform.Apps
{
    public interface IAppEnvironment : IManagedResource
    {
        long Id { get; }

        string Name { get; }

        // Variables
    }
}