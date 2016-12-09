using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace Carbon.Packaging
{
    using Storage;

    public class ZipPackage : Package
    {
        private readonly ZipArchive archive;
        private readonly bool stripFirstLevel;

        public ZipPackage(ZipArchive archive, bool stripFirstLevel = true)
        {
            #region Preconditions

            if (archive == null) throw new ArgumentNullException(nameof(archive));

            #endregion

            this.archive = archive;
            this.stripFirstLevel = stripFirstLevel;
        }

        public override IEnumerable<IBlob> Enumerate()
        {
            foreach (var entry in archive.Entries)
            {
                var key = GetKey(entry.FullName);

                // Skip directories & empty files
                if (string.IsNullOrWhiteSpace(key) || key.EndsWith("/") || entry.CompressedLength == 0) continue;

                yield return new ZipEntryBlob(key, entry);
            }
        }

        #region Helpers

        private string GetKey(string key)
        {
            if (stripFirstLevel)
            {
                var trim = key.Split(Seperators.ForwardSlash)[0] + '/';

                return key.Replace(trim, "");
            }

            return key;
        }

        #endregion

        public override void Dispose()
        {
            archive.Dispose();
        }

        public static async Task<ZipPackage> FetchAsync(Uri url, bool stripFirstLevel = true)
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

        public static ZipPackage FromStream(Stream stream, bool stripFirstLevel = true, bool leaveOpen = false)
        {
            #region Preconditions

            if (stream == null) throw new ArgumentNullException(nameof(stream));

            #endregion

            if (stream.CanSeek) stream.Position = 0;

            // Dispose the stream when we've disposed the archive
            var zip = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen);

            return new ZipPackage(zip, stripFirstLevel);
        }
    }
}