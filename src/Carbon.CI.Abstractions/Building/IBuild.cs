using System;

namespace Carbon.CI
{ 
    public interface IBuild 
    {
        long Id { get; }

        BuildStatus Status { get; }

        DateTime? Started { get; }

        DateTime? Completed { get; }

        string ResourceId { get; }
    }
}