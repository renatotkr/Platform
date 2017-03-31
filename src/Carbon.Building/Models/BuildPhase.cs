namespace Carbon.Builder
{
    public enum BuildPhase : byte
    {
        Downloading = 1,
        Building    = 2,
        Finalizing  = 3,
        Completed   = 4
    }
}
