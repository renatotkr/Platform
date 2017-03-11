namespace Carbon.Platform.Databases
{
    public enum DatabaseStatus : byte
    {
        Unknown     = 0,
        Online      = 1, // Running
        Offline     = 2,
        Recovering  = 3,
        Removed     = 4
    }
}