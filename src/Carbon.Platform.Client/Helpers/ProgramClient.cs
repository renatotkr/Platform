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
            this.api = api;
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
    }
}