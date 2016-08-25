using System;
using System.IO;
using System.Threading.Tasks;

namespace Carbon.Packaging
{
    using Data;
    using Storage;

    public class PackageStore : IPackageStore
    {
        private readonly IBlobStore blobStore;

        public PackageStore(IBlobStore blobStore)
        {
            this.blobStore = blobStore;
        }

        public async Task<Hash> PutAsync(Package package)
        {
            #region Preconditions

            if (package == null) throw new ArgumentNullException(nameof(package));

            #endregion

            var key = package.Name + "/" + package.Version; // + ".zip" ?

            using (var ms = new MemoryStream())
            {
                var hash = Hash.ComputeSHA256(ms, true);

                await package.ZipToAsync(ms).ConfigureAwait(false);

                ms.Seek(0, SeekOrigin.Begin);

                var blob = new Blob(ms) {
                    { "Content-Type", "application/zip" }
                };

                await blobStore.PutAsync(key, blob).ConfigureAwait(false);

                return hash;
            }
        }

        public async Task<Package> GetAsync(string name, Semver version)
        {
            var key = name + "/" + version.ToString();

            var ms = new MemoryStream();

            using (var blob = await blobStore.GetAsync(key).ConfigureAwait(false))
            {
                await blob.CopyToAsync(ms).ConfigureAwait(false);
            }

            ms.Seek(0, SeekOrigin.Begin);

            return ZipPackage.FromStream(ms, stripFirstLevel: false);
        }

        /*
        public async Task DownloadToAsync(string key, DirectoryInfo target)
        {
            using (var ms = new MemoryStream())
            {
                using (var blob = await blobStore.GetAsync(key).ConfigureAwait(false))
                {
                    await blob.CopyToAsync(ms).ConfigureAwait(false);
                }

                ms.Seek(0, SeekOrigin.Begin);

                using (var archive = new ZipArchive(ms, ZipArchiveMode.Read))
                {
                    archive.ExtractToDirectory(target.FullName);
                }
            }
        }
        */
    }
}

// var key = tag.Path + ".zip";  (e.g. app/2.1.1.zip)