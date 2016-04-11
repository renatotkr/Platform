using System;

namespace Carbon.Platform
{
    public interface IFrontend
    {
        string Name { get; }

        Uri RepositoryUrl { get; }
    }
}