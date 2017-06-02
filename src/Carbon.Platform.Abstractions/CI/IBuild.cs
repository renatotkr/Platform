using System;
using Carbon.Platform.Resources;

namespace Carbon.CI
{ 
    public interface IBuild : IManagedResource
    {
        BuildStatus Status { get; }

        DateTime? Started { get; }

        DateTime? Completed { get; }
    }
}