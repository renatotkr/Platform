using System;

using Carbon.Data.Protection;

namespace Carbon.Packaging
{
    public interface IManifestEntry
    {
        string Path { get; }

        Hash Hash { get; }

        DateTime Modified { get; }
    }
}