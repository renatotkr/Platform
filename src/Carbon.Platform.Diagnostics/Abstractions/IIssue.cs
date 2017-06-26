using System;

namespace Carbon.Platform.Diagnostics
{
    public interface IIssue
    {
        long Id { get; }

        int? LocationId { get; }
        
        DateTime? Resolved { get; }
    }
}