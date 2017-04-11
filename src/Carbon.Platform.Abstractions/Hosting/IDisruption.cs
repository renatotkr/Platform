using System;

namespace Carbon.Platform.Disruptions
{
    public interface IDisruption
    {
        long Id { get; }

        long LocationId { get; }
        
        // scope = buckets, ...

        DateTime? Resolved { get; }
    }
}