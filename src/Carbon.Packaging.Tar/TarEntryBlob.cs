using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using SharpCompress.Archives;

namespace Carbon.Packaging
{
    using SharpCompress.Archives.Tar;
    using Storage;

    internal class TarEntryBlob : IBlob
    {
        private readonly TarArchiveEntry entry;

        public TarEntryBlob(string name, TarArchiveEntry entry)
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

        public void WriteTo(Stream stream)
        {
            entry.WriteTo(stream);
        }

        public void Dispose()
        {
        }
    }
}