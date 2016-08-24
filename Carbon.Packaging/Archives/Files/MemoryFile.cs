using System;
using System.IO;

namespace Carbon.Packaging
{
    internal class MemoryFile : IFile
    {
        private readonly Stream stream;

        public MemoryFile(string name, Stream stream)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            #endregion

            Name = name;

            this.stream = stream;

            Modified = DateTime.UtcNow;
        }

        public string Name { get; }

        public DateTime Modified { get; }

        public Stream Open() => stream;
    }
}