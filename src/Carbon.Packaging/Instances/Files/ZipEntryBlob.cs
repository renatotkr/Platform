using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Carbon.Packaging
{
    using Storage;

    internal class ZipEntryBlob : IBlob
    {
        private readonly ZipArchiveEntry entry;

        public ZipEntryBlob(string key, ZipArchiveEntry entry)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));

            this.entry = entry ?? throw new ArgumentNullException(nameof(entry));
        }

        public string Key { get; }

        public long Size => entry.Length;

        public DateTime Modified => entry.LastWriteTime.UtcDateTime;

        public IReadOnlyDictionary<string, string> Properties => BlobProperties.Empty;

        public ValueTask<Stream> OpenAsync() => new ValueTask<Stream>(entry.Open());

        public void Dispose()
        {
        }
    }
}