using System;
using System.IO;
using System.Threading.Tasks;

namespace Carbon.Packaging
{
    using Protection;
    using Storage;

    public class ProtectedPackageStore
    {
        private readonly IBucket blobStore;
        private readonly byte[] password;

        public ProtectedPackageStore(IBucket blobStore, byte[] password)
        {
            #region Preconditions

            if (blobStore == null)
                throw new ArgumentNullException(nameof(blobStore));

            if (password == null)
                throw new ArgumentNullException(nameof(password));

            #endregion

            this.blobStore = blobStore;
            this.password = password;
        }

        public async Task<Hash> PutAsync(string name, Package package)
        {
            using (var ms = new MemoryStream())
            {
                await package.ZipToStreamAsync(ms).ConfigureAwait(false);

                ms.Position = 0;

                var hash = Hash.ComputeSHA256(ms, leaveOpen: true);

                var secret = SecretKey.Derive(password, hash.Data);

                var protector = new AesProtector(secret);

                using (var packageStream = protector.EncryptStream(ms))
                {
                    var blob = new Blob(packageStream) {
                        ContentType = "application/zip"
                    };

                    await blobStore.PutAsync(name, blob).ConfigureAwait(false);
                }

                return hash;
            }
        }

        public async Task<Package> GetAsync(string name, Hash hash)
        {
            var ms = new MemoryStream();

            using (var blob = await blobStore.GetAsync(name))
            {
                using (var data = blob.Open())
                {
                    await data.CopyToAsync(ms).ConfigureAwait(false);
                }
            }

            ms.Seek(0, SeekOrigin.Begin);

            var secret = SecretKey.Derive(password, hash.Data);

            var protector = new AesProtector(secret);

            var stream = protector.DecryptStream(ms);

            #region Verify the hash

            var computedHash = Hash.ComputeSHA256(stream, true);

            if (computedHash.ToHexString() != hash.ToHexString())
            {
                throw new IntegrityException(hash.Data, computedHash.Data);
            }

            stream.Position = 0;

            #endregion

            return ZipPackage.FromStream(stream, false);
        }
    }
}

// var key = tag.Path + ".zip";  (e.g. app/2.1.1.zip)