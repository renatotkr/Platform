using System;

namespace Carbon.Platform.Hosting
{
    [Flags]
    public enum DomainRecordFlags
    {
        None     = 0,
        Alias    = 1 << 0,
        Wildcard = 1 << 1
    }
}