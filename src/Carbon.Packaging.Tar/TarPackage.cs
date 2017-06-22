using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Carbon.Storage;
using SharpCompress.Archives;
using SharpCompress.Readers;

namespace Carbon.Packaging
{
    public class TarPackage : IPackage, IDisposable
    {
        private readonly IArchive archive;
        private readonly bool stripFirstLevel;

        public TarPackage(IArchive archive, bool stripFirstLevel = true)
        {
            this.archive = archive ?? throw new ArgumentNullException(nameof(archive));
            this.stripFirstLevel = stripFirstLevel;
        }

        public IEnumerable<IBlob> Enumerate()
        {
            foreach (var entry in archive.Entries)
            {
                var key = GetKey(entry.Key);

                // Skip directories & empty files
                if (string.IsNullOrWhiteSpace(key)
                    || key.EndsWith("/") 
                    || entry.Size == 0) continue;

                yield return new ArchiveEntryBlob(key, entry);
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
            bool isCompressed = true)
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

            var archive = ArchiveFactory.Open(targetStream, new ReaderOptions {
                LeaveStreamOpen = leaveStreamOpen,
            });
            
            return new TarPackage(archive, stripFirstLevel);
        }

        #region IPackage

        public IEnumerator<IBlob> GetEnumerator()
        {
            return Enumerate().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Enumerate().GetEnumerator();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            archive.Dispose();
        }

        #endregion
    }
}