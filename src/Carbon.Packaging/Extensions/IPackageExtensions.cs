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
        public static IBlob Find(this IPackage package, string absolutePath) // key?
        {
            foreach (var file in package)
            {
                if (file.Key == absolutePath)
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
                if (blob.Key.StartsWith(prefix))
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
                var filePath = Path.Combine(targetDirectory.FullName, item.Key.Replace('/', Path.DirectorySeparatorChar));

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

        public static async Task ToZipStreamAsync(
            this IPackage package,
            Stream stream, 
            bool leaveStreamOpen = false)
        {
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: leaveStreamOpen))
            {
                foreach (var packageEntry in package)
                {
                    var format = Path.GetExtension(packageEntry.Key).Trim(Seperators.Period);

                    var compressionLevel = FileFormat.IsText(format)
                        ? CompressionLevel.Optimal
                        : CompressionLevel.Fastest;

                    var archiveEntry = archive.CreateEntry(packageEntry.Key, compressionLevel);
                    
                    using (var targetStream = archiveEntry.Open())
                    using (var sourceStream = await packageEntry.OpenAsync().ConfigureAwait(false))
                    {
                        await sourceStream.CopyToAsync(targetStream).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}