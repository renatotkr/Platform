using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Carbon.Packaging
{
    using Storage;

    internal class ZipFile : IBlob
    {
        private readonly ZipArchiveEntry entry;

        public ZipFile(string name, ZipArchiveEntry entry)
        {
            #region Preconditions

            if (name == null) throw new ArgumentNullException(nameof(name));
            if (entry == null) throw new ArgumentNullException(nameof(entry));

            #endregion

            Name = name;

            this.entry = entry;
        }

        public string Name { get; }

        public DateTime Modified => entry.LastWriteTime.UtcDateTime;

        public long Size => entry.Length;

        public Stream Open() => entry.Open();

        IDictionary<string, string> IBlob.Metadata => null;

        public void Dispose() { }
    }
}