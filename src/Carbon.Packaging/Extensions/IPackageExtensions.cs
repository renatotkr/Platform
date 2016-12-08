using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Carbon.Packaging
{
    using Storage;

    public static class IPackageExtensions
    {
        public static async Task ExtractToDirectoryAsync(this IPackage package, DirectoryInfo target)
        {
            #region Preconditions

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (target.Exists)
                throw new Exception("Target directory already exists.");

            #endregion

            target.Create();

            foreach (var item in package.Enumerate())
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

        public static async Task ZipToStreamAsync(this IPackage package, Stream stream)
        {
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
            {
                foreach (var item in package.Enumerate())
                {
                    var format = Path.GetExtension(item.Name).Trim(Seperators.Period);

                    var compressionLevel = FileFormat.IsText(format)
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
    }
}