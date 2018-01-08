namespace Carbon.Platform
{
    public enum LocationStatus : byte
    {
        Unknown   = 0,
        Healthy   = 1,
        Unhealthy = 2   // One or more services are disrupted
    }
}