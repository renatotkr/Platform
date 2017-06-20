using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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

        public TarPackage(TarArchive archive, bool stripFirstLevel = true)
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
                    || entry.CompressedSize == 0) continue;

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

        public static async Task<TarPackage> FetchAsync(Uri url, bool stripFirstLevel = true)
        {
            #region Preconditions

            if (url == null) throw new ArgumentNullException(nameof(url));

            #endregion

            using (var httpClient = new HttpClient())
            {
                using (var httpStream = await httpClient.GetStreamAsync(url).ConfigureAwait(false))
                {
                    var ms = new MemoryStream();

                    await httpStream.CopyToAsync(ms).ConfigureAwait(false);

                    return FromStream(ms);
                }
            }
        }

        public static TarPackage FromStream(
            Stream stream,
            bool stripFirstLevel = true, 
            bool leaveOpen = false)
        {
            #region Preconditions

            if (stream == null) throw new ArgumentNullException(nameof(stream));

            #endregion

            if (stream.CanSeek) stream.Position = 0;

            // Dispose the stream when we've disposed the archive
            var archive = TarArchive.Open(stream, new ReaderOptions {
                LeaveStreamOpen = leaveOpen
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