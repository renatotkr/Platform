using System;
using System.Collections.Generic;
using System.IO;

namespace Carbon.Packaging
{
    using Storage;

    internal class MemoryFile : IBlob
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

        public long Size => stream.Length;

        public Stream Open() => stream;

        IDictionary<string, string> IBlob.Metadata => null;

        public void Dispose()
        {
            stream.Dispose();
        }
    }
}