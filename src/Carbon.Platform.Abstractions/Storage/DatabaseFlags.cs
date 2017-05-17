namespace Carbon.Platform.Storage
{
    public enum DatabaseFlags
    {
        Primary   = 1 << 1,
        Secondary = 1 << 2,
        ReadOnly  = 1 << 3,
        Replica   = 1 << 4
    }
}