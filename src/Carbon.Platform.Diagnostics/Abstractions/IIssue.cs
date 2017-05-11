using System;

namespace Carbon.Platform.Diagnostics
{
    public interface IIssue
    {
        long Id { get; }

        int? LocationId { get; }
        
        // scope = buckets, ...

        DateTime? Resolved { get; }
    }
}