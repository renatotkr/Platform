using System;
using System.IO;
using System.Threading.Tasks;

namespace Carbon.Packaging
{
    using Protection;
    using Storage;
    using Versioning;

    public class ProtectedPackageStore
    {
        private readonly IBucket bucket;
        private readonly byte[] password;
        private readonly string prefix;

        public ProtectedPackageStore(IBucket bucket, byte[] password, string prefix = null)
        {
            #region Preconditions

            if (bucket == null)
                throw new ArgumentNullException(nameof(bucket));

            if (password == null)
                throw new ArgumentNullException(nameof(password));

            #endregion

            this.bucket = bucket;
            this.password = password;

            if (prefix != null)
            {
                if (!prefix.EndsWith("/"))
                {
                    prefix = prefix + "/";
                }

                this.prefix = prefix;
            }
            else
            {
                this.prefix = "";
            }
        }

        public async Task<Hash> PutAsync(long id, SemanticVersion version, IPackage package)
        {
            var key = prefix + id.ToString() + "/" + version.ToString();

            using (var ms = new MemoryStream())
            {
                await package.ZipToStreamAsync(ms).ConfigureAwait(false);

                ms.Position = 0;

                var hash = Hash.ComputeSHA256(ms, leaveOpen: true);

                var secret = SecretKey.Derive(password, hash.Data);

                using (var protector = new AesProtector(secret))
                {
                    using (var packageStream = protector.EncryptStream(ms))
                    {
                        var blob = new Blob(packageStream) {
                            ContentType = "application/zip"
                        };

                        await bucket.PutAsync(key, blob).ConfigureAwait(false);
                    }
                }

                return hash;
            }
        }

        public async Task<Package> GetAsync(long id, SemanticVersion version, Hash hash)
        {
            var key = prefix + id.ToString() + "/" + version.ToString();

            var ms = new MemoryStream();

            using (var blob = await bucket.GetAsync(key))
            {
                using (var data = blob.Open())
                {
                    await data.CopyToAsync(ms).ConfigureAwait(false);
                }
            }

            ms.Position = 0;
            
            var secret = SecretKey.Derive(password, hash.Data);

            var protector = new AesProtector(secret); // dispose?

            var stream = protector.DecryptStream(ms);

            #region Verify the hash

            var computedHash = Hash.ComputeSHA256(stream, true);

            if (computedHash != hash)
            {
                throw new IntegrityException(hash.Data, computedHash.Data);
            }

            #endregion

            return ZipPackage.FromStream(stream, false);
        }
    }
}

// var key = tag.Path + ".zip";  (e.g. app/2.1.1.zip)