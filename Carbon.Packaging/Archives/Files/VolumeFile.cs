using System;
using System.IO;

namespace Carbon.Packaging
{
    using Data;
    using Storage;

    internal class VolumeFile : IBlob
    {
        private readonly FileInfo file;

        public VolumeFile(string name, FileInfo file)
        {
            #region Preconditions

            if (file == null)
                throw new ArgumentNullException(nameof(file));

            if (!file.Exists)
                throw new ArgumentException($"'{file.FullName}' does not exist.", nameof(file));

            #endregion

            Name = name;

            this.file = file;
        }

        public string Name { get; }

        public DateTime Modified => file.LastWriteTimeUtc;

        public long Size => file.Length;

        public Stream Open() => file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
    }
}