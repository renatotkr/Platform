using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Carbon.Packaging
{
    using Storage;

    internal class ZipEntryBlob : IBlob
    {
        private readonly ZipArchiveEntry entry;

        public ZipEntryBlob(string name, ZipArchiveEntry entry)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            #endregion

            Name = name;

            this.entry = entry;
        }

        public Stream Open() => entry.Open();

        public string Name { get; }

        public long Size => entry.Length;

        public DateTime Modified => entry.LastWriteTime.UtcDateTime;

        public IDictionary<string, string> Metadata => null;

        public void Dispose()
        {
        }
    }
}