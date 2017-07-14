using System;

using Carbon.Data.Protection;

namespace Carbon.Packaging
{
    public interface IManifestEntry
    {
        string Key { get; }

        Hash Hash { get; }

        DateTime Modified { get; }
    }
}