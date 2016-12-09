using System;
using System.IO;
using System.Threading.Tasks;

namespace Carbon.Packaging
{
    using Protection;
    using Storage;
    using Versioning;

    public class PackageStore : IPackageStore
    {
        private readonly IBucket bucket;

        public PackageStore(IBucket bucket)
        {
            this.bucket = bucket;
        }

        public async Task<Hash> PutAsync(long id, SemanticVersion version, IPackage package)
        {
            #region Preconditions

            if (package == null) throw new ArgumentNullException(nameof(package));

            #endregion

            var key = id.ToString() + "/" + version.ToString();

            using (var ms = new MemoryStream())
            {
                await package.ZipToStreamAsync(ms).ConfigureAwait(false);

                var hash = Hash.ComputeSHA256(ms, leaveOpen: true);

                var blob = new Blob(ms) {
                    ContentType = "application/zip"
                };

                await bucket.PutAsync(key, blob).ConfigureAwait(false);

                return hash;
            }
        }

        public async Task<IPackage> GetAsync(long id, SemanticVersion version)
        {
            var key = id.ToString() + "/" + version.ToString();

            var ms = new MemoryStream();

            using (var blob = await bucket.GetAsync(key).ConfigureAwait(false))
            {
                using (var channel = blob.Open())
                {
                    await channel.CopyToAsync(ms).ConfigureAwait(false);
                }
            }

            ms.Position = 1;

            return ZipPackage.FromStream(ms, stripFirstLevel: false);
        }     
    }
}

// var key = tag.Path + ".zip";  (e.g. app/2.1.1.zip)