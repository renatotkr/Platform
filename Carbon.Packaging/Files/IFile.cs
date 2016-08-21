using System;
using System.IO;

namespace Carbon.Packaging
{
    public interface IFile
    {
        string Name { get; }

        DateTime Modified { get; }

        Stream Open(); // OpenAsync?
    }
}