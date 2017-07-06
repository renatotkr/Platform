using System;
using System.IO;
using System.Threading.Tasks;

using Carbon.Packaging;
using Carbon.Platform.Computing;
using Carbon.Storage;
using Carbon.Versioning;

namespace Carbon.Platform
{
    public class ProgramClient
    {
        private readonly ApiBase api;

        internal ProgramClient(ApiBase api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public Task<ProgramDetails> GetAsync(long id)
        {
            return api.GetAsync<ProgramDetails>($"/programs/{id}");
        }

        public Task<ProgramDetails> GetAsync(long id, SemanticVersion version)
        {
            return api.GetAsync<ProgramDetails>($"/programs/{id}@{version}");
        }

        public async Task<IPackage> DownloadAsync(long id, SemanticVersion version)
        {
            var packageStream = await api.DownloadAsync(
                $"/programs/{id}@{version}/package.zip");

            return ZipPackage.FromStream(packageStream, true);
        }

        public async Task<ProgramDetails> UploadAsync(long id, SemanticVersion version, Package package)
        {
            #region Preconditions

            if (package == null)
                throw new ArgumentNullException(nameof(package));

            #endregion

            var stream = new MemoryStream();

            await package.ToZipStreamAsync(stream, leaveStreamOpen: true);

            stream.Position = 0;

            return await api.UploadAsync<ProgramDetails>(
                path        : $"/programs/{id}@{version}/package",
                contentType : "application/zip",
                stream      : stream
            );
        }
    }
}