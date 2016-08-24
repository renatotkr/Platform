using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Carbon.Packaging
{
    public abstract class Package : IEnumerable<IFile>, IPackage, IDisposable
    {
        private string name;
        private Semver version;

        public abstract IEnumerable<IFile> Enumerate();

        public abstract void Dispose();

        public long Id { get; set; }

        public string Name => name;

        public Semver Version => version;

        public IList<PackageDependency> Dependencies => new List<PackageDependency>();

        public void Set(string name, Semver version)
        {
            this.name = name;
            this.version = version;
        }

        public IFile Find(string absolutePath) =>
            this.FirstOrDefault(item => item.Name == absolutePath);


        public IFile[] List(string prefix)
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
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
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

        public static Package FromDirectory(DirectoryInfo root) => new DirectoryPackage(root);

        #endregion

        #region IEnumerable

        IEnumerator<IFile> IEnumerable<IFile>.GetEnumerator()
            => Enumerate().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => Enumerate().GetEnumerator();

        #endregion
    }
}