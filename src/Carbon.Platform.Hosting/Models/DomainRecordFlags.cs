using System;

namespace Carbon.Platform.Hosting
{
    [Flags]
    public enum DomainRecordFlags
    {
        None        = 0,
        Authoritive = 1 << 0,
        Alias       = 1 << 1,
        Wildcard    = 1 << 2
    }
}