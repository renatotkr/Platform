namespace Carbon.Platform
{
    public interface ILocation : IManagedResource
    {
        long Id { get; }

        // e.g. us-east-1, us-east-1a

        string Name { get; }
    }
}