namespace Carbon.CI
{
    public enum BuildStatus : byte
    {
        Pending    = 1,
        Succeeded  = 2,
        Failed     = 3
    }
}

/*
Pending    = 1,
Building   = 2,
Completed  = 3,
Failed     = 5
*/