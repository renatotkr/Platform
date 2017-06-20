using System;
using System.IO;
using System.Threading.Tasks;

using Carbon.Data.Protection;
using Carbon.Storage;

namespace Carbon.Packaging
{
    public class PackageStore : IPackageStore
    {
        private readonly IBucket bucket;

        public PackageStore(IBucket bucket)
        {
            this.bucket = bucket ?? throw new ArgumentNullException(nameof(bucket));
        }

        public async Task<PutPackageResult> PutAsync(
            string key, 
            IPackage package,
            PutPackageOptions? options = null)
        {
            #region Preconditions

            if (package == null) throw new ArgumentNullException(nameof(package));

            #endregion

            using (var ms = new MemoryStream())
            {
                await package.ZipToStreamAsync(ms).ConfigureAwait(false);

                var hash = Hash.ComputeSHA256(ms, leaveOpen: true);

                var blob = new Blob(key, ms, new BlobMetadata {
                    ContentType = "application/zip"
                });

                await bucket.PutAsync(blob, new PutBlobOptions {
                    EncryptionKey = options?.EncryptionKey
                }).ConfigureAwait(false);

                return new PutPackageResult(key, hash.Data);
            }
        }

        public async Task<IPackage> GetAsync(string key, GetPackageOptions? options = null)
        {
            var ms = new MemoryStream();

            var blobOptions = new GetBlobOptions {
                EncryptionKey = options?.EncryptionKey
            };

            using (var blob = await bucket.GetAsync(key, blobOptions).ConfigureAwait(false))
            using (var blobStream = await blob.OpenAsync().ConfigureAwait(false))
            {
                await blobStream.CopyToAsync(ms).ConfigureAwait(false);
            }

            ms.Position = 0;

            return ZipPackage.FromStream(ms, stripFirstLevel: false);
        }
    }
}