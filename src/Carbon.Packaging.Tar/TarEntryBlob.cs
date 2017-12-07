using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Carbon.Storage;

using SharpCompress.Archives.Tar;

namespace Carbon.Packaging
{
    internal class TarEntryBlob : IBlob
    {
        private readonly TarArchiveEntry entry;

        public TarEntryBlob(string key, TarArchiveEntry entry)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));

            this.entry = entry ?? throw new ArgumentNullException(nameof(entry));
        }

        public string Key { get; }

        public long Size => entry.Size;

        public DateTime Modified => entry.LastModifiedTime?.ToUniversalTime() ?? DateTime.UtcNow;

        public IReadOnlyDictionary<string, string> Properties => BlobProperties.Empty;

        public ValueTask<Stream> OpenAsync()
        {
            return new ValueTask<Stream>(entry.OpenEntryStream());
        }

        public void Dispose()
        {
        }
    }
}