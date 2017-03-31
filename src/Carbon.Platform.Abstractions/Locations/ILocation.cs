namespace Carbon.Platform
{
    public interface ILocation
    {
        long Id { get; }

        // e.g. us-east-1, us-east-1a

        string Name { get; }

        int ProviderId { get; }
    }
}