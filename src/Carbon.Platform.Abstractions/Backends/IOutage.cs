using System;

namespace Carbon.Platform
{
    public interface IOutage
    {
        long Id { get; }

        long LocationId { get; }

        string[] Scope { get; }

        DateTime? Resolved { get; }
    }
}