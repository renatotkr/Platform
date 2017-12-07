using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Carbon.Storage;
using SharpCompress.Archives.Tar;
using SharpCompress.Readers;

namespace Carbon.Packaging
{
    public class TarPackage : IPackage, IDisposable
    {
        private readonly TarArchive archive;
        private readonly bool stripFirstLevel;
        private readonly TarArchiveEntry[] entries;

        public TarPackage(TarArchive archive, bool stripFirstLevel = true)
        {
            this.archive = archive ?? throw new ArgumentNullException(nameof(archive));
            this.stripFirstLevel = stripFirstLevel;
            
            // This is required to prevent the stream from being repositioned during enumeration
            // see: https://github.com/adamhathcock/sharpcompress/issues/116

            this.entries = archive.Entries.ToArray();
        }

        public IEnumerable<IBlob> Enumerate()
        {
            foreach (var entry in entries)
            {
                var key = GetKey(entry.Key);

                // Skip directories & empty files
                if (string.IsNullOrWhiteSpace(key)
                    || key.EndsWith("/") 
                    || entry.Size == 0) continue;

                yield return new TarEntryBlob(key, entry);
            }
        }

        #region Helpers

        private string GetKey(string key)
        {
            if (stripFirstLevel)
            {
                var trim = key.Split('/')[0] + '/';

                return key.Replace(trim, "");
            }

            return key;
        }

        #endregion

        public static async Task<TarPackage> OpenAsync(
            Stream stream,
            bool stripFirstLevel = true, 
            bool leaveStreamOpen = false,
            bool isCompressed    = true)
        {
            #region Preconditions

            if (stream == null) throw new ArgumentNullException(nameof(stream));

            #endregion

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            Stream targetStream;
            
            // Decompress before opening...

            if (isCompressed)
            {
                using (var decompressionStream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    targetStream = new MemoryStream();

                    await decompressionStream.CopyToAsync(targetStream).ConfigureAwait(false);

                    targetStream.Position = 0;
                }
            }
            else
            {
                targetStream = stream;
            }

            var archive = TarArchive.Open(targetStream, new ReaderOptions {
                LeaveStreamOpen = leaveStreamOpen,
                LookForHeader   = true
            });
            
            return new TarPackage(archive, stripFirstLevel);
        }

        #region IPackage

        public IEnumerator<IBlob> GetEnumerator() => Enumerate().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Enumerate().GetEnumerator();

        #endregion

        #region IDisposable

        public void Dispose()
        {
            archive.Dispose();
        }

        #endregion
    }
}