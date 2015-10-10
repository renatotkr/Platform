namespace Carbon.Platform
{
    using System;
    using System.IO;
    using System.IO.Compression;

    public static class StreamExtensions
    {
        public static void GZip(this Stream sourceStream, Stream targetStream, bool leaveOpen = true)
        {
            using (var compressedStream = new GZipStream(targetStream, CompressionMode.Compress, leaveOpen))
            {
                sourceStream.CopyTo(compressedStream);
            }
        }

        public static MemoryStream ToGZipStream(this Stream stream, bool leaveOpen = true)
        {
            #region Preconditions

            if (stream == null) throw new ArgumentNullException("stream");

            #endregion

            try
            {
                if (stream.CanSeek && stream.Position != 0)
                {
                    stream.Position = 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error setting position", ex);
            }

            var ms = new MemoryStream();

            using (var compressedStream = new GZipStream(ms, CompressionMode.Compress, leaveOpen))
            {
                stream.CopyTo(compressedStream);
            }

            ms.Position = 0;

            return ms;
        }
    }
}