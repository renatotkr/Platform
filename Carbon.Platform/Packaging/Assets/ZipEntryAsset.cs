namespace Carbon.Platform
{
    using System;
    using System.IO;
    using System.IO.Compression;

    internal class ZipEntryAsset : IAsset
    {
        private readonly ZipArchiveEntry entry;

        public ZipEntryAsset(string name, ZipArchiveEntry entry)
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

        public Stream Open() => entry.Open();
    }
}
