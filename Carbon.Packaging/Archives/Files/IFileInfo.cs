using System;

namespace Carbon.Packaging
{
    using Data;

    public interface IFileInfo
    {
        string Name { get; }    // e.g. img/c.gif

        DateTime Modified { get; }

        CryptographicHash Hash { get; } // sha256
    }
}
