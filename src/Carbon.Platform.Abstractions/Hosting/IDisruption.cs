using System;

namespace Carbon.Platform.Disruptions
{
    public interface IDisruption
    {
        long Id { get; }

        int LocationId { get; }
        
        // scope = buckets, ...

        DateTime? Resolved { get; }
    }
}