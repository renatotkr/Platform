namespace Carbon.Platform
{
    public interface ILocation
    {
        int Id { get; }

        // e.g. us-east-1, us-east-1a
        string Name { get; }

        LocationType Type { get; }
    }

    // Locations may span the globe, mutiple regions, a single region, or a zone 
}