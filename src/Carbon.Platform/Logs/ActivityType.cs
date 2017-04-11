namespace Carbon.Platform.Logs
{
    public enum ActivityType
    {
        Create   = 1,
        Build    = 2,
        Publish  = 3,  // New Version
        Deploy   = 4,  // On instance

        Add      = 7, // Instance
        Remove   = 8, // Instance
    }
}