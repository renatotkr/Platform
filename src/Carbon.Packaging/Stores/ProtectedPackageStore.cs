using System;
using System.IO;
using System.Threading.Tasks;

using Carbon.Data.Protection;
using Carbon.Storage;

namespace Carbon.Packaging
{
    public class ProtectedPackageStore
    {
        private readonly IBucket bucket;

        public ProtectedPackageStore(IBucket bucket)
        {
            this.bucket = bucket ?? throw new ArgumentNullException(nameof(bucket));
        }

        public async Task<PutPackageResult> PutAsync(
            string name,
            IPackage package,
            Secret encryptionKey)
        {
            var iv = Secret.Generate(16);

            using (var ms = new MemoryStream())
            {
                await package.ZipToStreamAsync(ms).ConfigureAwait(false);

                ms.Position = 0;

                var hash = Hash.ComputeSHA256(ms, leaveOpen: true);

                using (var protector = new AesDataProtector(encryptionKey.Value, iv.Value))
                using (var encryptedPackageStream = protector.EncryptStream(ms))
                {
                    var blob = new Blob(name, encryptedPackageStream, new BlobMetadata {
                        ContentType = "application/zip"
                    });

                    await bucket.PutAsync(blob).ConfigureAwait(false);
                }

                return new PutPackageResult(name, iv.Value, hash);
            }
        }

        public async Task<Package> GetAsync(
            string name,
            Secret encryptionKey,
            byte[] iv)
        {
            var ms = new MemoryStream();

            using (var blob = await bucket.GetAsync(name))
            using (var blobStream = await blob.OpenAsync().ConfigureAwait(false))
            {
                await blobStream.CopyToAsync(ms).ConfigureAwait(false);
            }

            ms.Position = 0;
            
            var protector = new AesDataProtector(encryptionKey.Value, iv); // dispose?
            
            return ZipPackage.FromStream(protector.DecryptStream(ms), false);
        }
    }
}