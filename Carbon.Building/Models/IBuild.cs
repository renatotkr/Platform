using System;

namespace Carbon.Building
{
    public interface IBuild
    {
        long Id { get; }

        BuildStatus Status { get; }

        DateTime? Started { get; }

        DateTime? Completed { get; }
    }
}