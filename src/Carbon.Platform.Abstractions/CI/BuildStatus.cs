namespace Carbon.Platform.CI
{
    public enum BuildStatus : byte
    {
        Pending    = 1,
        Building   = 2,
        Completed  = 3,
        Failed     = 5
    }
}