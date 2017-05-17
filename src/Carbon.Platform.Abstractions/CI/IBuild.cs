using System;
using Carbon.Platform.Resources;

namespace Carbon.Platform.CI
{ 
    public interface IBuild : IResource
    {
        BuildStatus Status { get; }

        DateTime? Started { get; }

        DateTime? Completed { get; }
    }
}