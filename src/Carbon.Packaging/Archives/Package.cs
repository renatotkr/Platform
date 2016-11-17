using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Carbon.Packaging
{
    using Storage;

    public abstract class Package : IEnumerable<IBlob>, IDisposable
    {
        public abstract IEnumerable<IBlob> Enumerate();

        public abstract void Dispose();

        public IBlob Find(string absolutePath) =>
            this.FirstOrDefault(item => item.Name == absolutePath);

        public IBlob[] List(string prefix)
            => this.Where(item => item.Name.StartsWith(prefix)).ToArray();

        public async Task ExtractToDirectoryAsync(DirectoryInfo target)
        {
            #region Preconditions

            if (target == null) throw new ArgumentNullException(nameof(target));

            if (target.Exists) throw new Exception("Target directory already exists.");

            #endregion

            target.Create();

            foreach (var item in Enumerate())
            {
                var filePath = Path.Combine(target.FullName, item.Name.Replace('/', Path.DirectorySeparatorChar));

                var file = new FileInfo(filePath);

                if (!file.Directory.Exists)
                {
                    try
                    {
                        file.Directory.Create();
                    }
                    catch { }
                }

                using (var targetStream = file.Open(FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    using (var sourceStream = item.Open())
                    {
                        await sourceStream.CopyToAsync(targetStream).ConfigureAwait(false);
                    }
                }
            }
        }

        public async Task ZipToAsync(Stream stream)
        {
            using (var archive = new System.IO.Compression.ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                foreach (var item in Enumerate())
                {
                    var format = Path.GetExtension(item.Name).Trim('.');

                    var compressionLevel = FileHelper.IsText(format)
                        ? CompressionLevel.Optimal
                        : CompressionLevel.NoCompression;

                    var entry = archive.CreateEntry(item.Name, compressionLevel);

                    using (var targetStream = entry.Open())
                    {
                        using (var sourceStream = item.Open())
                        {
                            await sourceStream.CopyToAsync(targetStream).ConfigureAwait(false);
                        }
                    }
                }
            }
        }

        #region Helpers

        public static Package FromDirectory(DirectoryInfo root) 
            => new DirectoryPackage(root);

        #endregion

        #region IEnumerable

        IEnumerator<IBlob> IEnumerable<IBlob>.GetEnumerator()
            => Enumerate().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => Enumerate().GetEnumerator();

        #endregion
    }
}