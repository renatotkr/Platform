using System;
using Carbon.Data.Sequences;

namespace Carbon.Platform.Diagnostics
{
    public interface IIssue
    {
        // environmentId | timestamp/ms | #
        BigId Id { get; }

        int? LocationId { get; }
        
        DateTime? Resolved { get; }
    }
}