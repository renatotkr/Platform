using System.IO;
using System.IO.Compression;

namespace Carbon.Packaging
{
    internal class ZippedBlob : PackageFile
    {
        private readonly ZipArchiveEntry entry;

        public ZippedBlob(string name, ZipArchiveEntry entry)
            : base(name, entry.Length, entry.LastWriteTime.UtcDateTime)
        {
            this.entry = entry;
        }

        public override Stream Open() => entry.Open();
    }
}