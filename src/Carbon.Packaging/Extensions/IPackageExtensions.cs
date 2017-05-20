using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

using Carbon.Extensions;
using Carbon.Storage;

namespace Carbon.Packaging
{
    public static class IPackageExtensions
    {
        public static IBlob Find(this IPackage package, string absolutePath)
        {
            foreach (var file in package)
            {
                if (file.Name == absolutePath)
                {
                    return file;
                }
            }

            return null;
        }

        public static IEnumerable<IBlob> Filter(this IPackage package, string prefix)
        {
            foreach (var blob in package)
            {
                if (blob.Name.StartsWith(prefix))
                {
                    yield return blob;
                }
            }
        }

        public static async Task ExtractToDirectoryAsync(this IPackage package, DirectoryInfo targetDirectory)
        {
            #region Preconditions

            if (targetDirectory == null)
                throw new ArgumentNullException(nameof(targetDirectory));

            if (targetDirectory.Exists)
                throw new Exception("Target directory already exists.");

            #endregion

            targetDirectory.Create();

            foreach (var item in package)
            {
                var filePath = Path.Combine(targetDirectory.FullName, item.Name.Replace('/', Path.DirectorySeparatorChar));

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
                    using (var sourceStream = await item.OpenAsync().ConfigureAwait(false))
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
                foreach (var item in package)
                {
                    var format = Path.GetExtension(item.Name).Trim(Seperators.Period);

                    var compressionLevel = FileFormat.IsText(format)
                        ? CompressionLevel.Optimal
                        : CompressionLevel.NoCompression;

                    var entry = archive.CreateEntry(item.Name, compressionLevel);

                    using (var targetStream = entry.Open())
                    {
                        using (var sourceStream = await item.OpenAsync().ConfigureAwait(false))
                        {
                            await sourceStream.CopyToAsync(targetStream).ConfigureAwait(false);
                        }
                    }
                }
            }
        }
    }
}