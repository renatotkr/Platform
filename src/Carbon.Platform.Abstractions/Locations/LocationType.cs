namespace Carbon.Platform
{
    public enum LocationType : byte
    {
        Global      = 1, // e.g. earth
        MultiRegion = 2, // e.g. us
        Region      = 3, // e.g. us-east-1
        Zone        = 4  // e.g. us-east-1-a
    }
}