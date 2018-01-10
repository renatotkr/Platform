using System;
using System.IO;
using System.Threading.Tasks;

namespace Carbon.Building.Web
{
    internal static class StreamExtensions
    {
        public static async Task CopyToFileAsync(this Stream stream, string destinationFilePath)
        {
            #region Preconditions

            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (destinationFilePath == null)
                throw new ArgumentNullException(nameof(destinationFilePath));

            #endregion

            #region Ensure the directory exists

            var di = new DirectoryInfo(Path.GetDirectoryName(destinationFilePath));

            if (!di.Exists) di.Create();

            #endregion

            using (var writeStream = new FileStream(destinationFilePath, FileMode.CreateNew))
            {
                await stream.CopyToAsync(writeStream).ConfigureAwait(false);
            }
        }
    }
}