using System;
using System.IO;

namespace Carbon.Packaging
{
    public interface IFile // IBlob?
    {
        string Name { get; }

        DateTime Modified { get; }

        Stream Open(); // OpenAsync?
    }
}