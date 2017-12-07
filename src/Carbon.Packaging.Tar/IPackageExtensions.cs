using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

using Carbon.Storage;
using SharpCompress.Writers;

namespace Carbon.Packaging
{
    public static class IPackageExtensions
    {
        public static async Task ToTarStreamAsync(
            this IPackage package, 
            Stream stream, 
            bool leaveStreamOpen = false)
        {
            #region Preconditions

            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            #endregion

            using (var compressedStream = new GZipStream(stream, CompressionMode.Compress))
            using (var writer = WriterFactory.Open(compressedStream,
                SharpCompress.Common.ArchiveType.Tar, 
                new WriterOptions(SharpCompress.Common.CompressionType.None) {
                    LeaveStreamOpen = leaveStreamOpen
                }))
            {
                foreach (var item in package)
                {
                    var format = Path.GetExtension(item.Key).Trim('.');

                    var ms = new MemoryStream();

                    using (var itemStream = await item.OpenAsync().ConfigureAwait(false))
                    {
                        Stream tempStream;

                        if (!itemStream.CanSeek)
                        {
                            tempStream = new MemoryStream();

                            await itemStream.CopyToAsync(tempStream);

                            tempStream.Position = 0;
                        }
                        else
                        {
                            tempStream = itemStream;
                        }

                        using (tempStream)
                        {
                            writer.Write(
                                filename         : item.Key,
                                source           : tempStream,
                                modificationTime : item.Modified
                            );
                        }
                    }
                }                
            }
        }
    }
}