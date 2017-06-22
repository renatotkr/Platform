using System.IO;
using System.Threading.Tasks;

using Carbon.Storage;
using SharpCompress.Archives.Tar;
using SharpCompress.Writers;

namespace Carbon.Packaging
{
    public static class IPackageExtensions
    {
        public static async Task ToTarAsync(
            this IPackage package, 
            Stream stream, 
            bool leaveStreamOpen = false)
        {
            using (var archive = TarArchive.Create())
            {
                foreach (var item in package)
                {
                    var format = Path.GetExtension(item.Name).Trim('.');

                    var entry = archive.AddEntry(
                        key         : item.Name, 
                        source      : await item.OpenAsync().ConfigureAwait(false),
                        closeStream : true
                    );
                }

                archive.SaveTo(stream, new WriterOptions(SharpCompress.Common.CompressionType.GZip) {
                    LeaveStreamOpen = leaveStreamOpen
                });
            }
        }
    }
}