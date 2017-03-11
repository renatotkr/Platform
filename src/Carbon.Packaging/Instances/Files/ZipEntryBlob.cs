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
            Name = name ?? throw new ArgumentNullException(nameof(name));

            this.entry = entry ?? throw new ArgumentNullException(nameof(entry));
        }

        public string Name { get; }

        public long Size => entry.Length;

        public DateTime Modified => entry.LastWriteTime.UtcDateTime;

        public IReadOnlyDictionary<string, string> Metadata => null;

        public Stream Open() => entry.Open();

        public void Dispose()
        {
        }
    }
}