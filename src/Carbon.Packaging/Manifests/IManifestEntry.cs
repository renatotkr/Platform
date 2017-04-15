using Carbon.Protection;

using System;

namespace Carbon.Packaging
{
    public interface IManifestEntry
    {
        string Path { get; }

        Hash Hash { get; }

        DateTime Modified { get; }
    }
}