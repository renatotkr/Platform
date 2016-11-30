﻿namespace Carbon.Platform
{
    public interface IFrontend
    {
        long Id { get; }

        SemanticVersion Version { get; }

        string Name { get; }
    }
}