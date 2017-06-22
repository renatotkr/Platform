using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

using Carbon.Storage;

using SharpCompress.Archives.Tar;
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
            using (var compressedStream = new GZipStream(stream, CompressionMode.Compress))
            using (var writer = WriterFactory.Open(compressedStream, SharpCompress.Common.ArchiveType.Tar, new WriterOptions(SharpCompress.Common.CompressionType.None) {
                LeaveStreamOpen = leaveStreamOpen,
            }))
            {
                foreach (var item in package)
                {
                    var format = Path.GetExtension(item.Name).Trim('.');

                    writer.Write(
                        filename         : item.Name,
                        source           : await item.OpenAsync().ConfigureAwait(false),
                        modificationTime : item.Modified
                    );
                }

                
            }
        }
    }
}