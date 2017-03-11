namespace Carbon.Platform.Storage
{
    public enum VolumeStatus : byte
    {
        Pending     = 0,
        Online      = 1,
        Unavailable = 2,
        Detached    = 3,
        Faulted     = 4,
        Removed     = 5
    }
}