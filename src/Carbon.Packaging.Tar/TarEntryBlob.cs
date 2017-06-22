using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using SharpCompress.Archives;

namespace Carbon.Packaging
{
    using Storage;

    internal class ArchiveEntryBlob : IBlob
    {
        private readonly IArchiveEntry entry;

        public ArchiveEntryBlob(string name, IArchiveEntry entry)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            this.entry = entry ?? throw new ArgumentNullException(nameof(entry));
        }

        public string Name { get; }

        public long Size => entry.Size;

        public DateTime Modified => entry.LastModifiedTime?.ToUniversalTime() ?? DateTime.UtcNow;

        public IReadOnlyDictionary<string, string> Metadata => null;

        public ValueTask<Stream> OpenAsync()
        {
            return new ValueTask<Stream>(entry.OpenEntryStream());
        }

        public void Dispose()
        {
        }
    }
}