using System;

namespace Carbon.Rds
{
    [Flags]
    public enum DatabaseEndpointFlags
    {
        None     = 0,
        Primary  = 1 << 0,
        ReadOnly = 1 << 4
    }
}