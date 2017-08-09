using System;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Packaging;
using Carbon.Platform.Storage;
using Carbon.Storage;

namespace Carbon.Platform
{
    public class RepositoryClient
    {
        private readonly ApiBase api;

        internal RepositoryClient(ApiBase api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public Task<RepositoryDetails[]> ListAsync(Expression filter = null)
        {
            return api.GetListAsync<RepositoryDetails>($"/repositories" + filter?.ToQueryString());
        }

        public Task<RepositoryDetails> GetAsync(long id)
        {
            return api.GetAsync<RepositoryDetails>($"/repositories/{id}");
        }
        
        public Task<RepositoryDetails> GetAsync(long id, string revision)
        {
            return api.GetAsync<RepositoryDetails>($"/repositories/{id}@{revision}");
        }

        public async Task<IPackage> DownloadAsync(long id, string revision = "master")
        {
            var packageStream = await api.DownloadAsync($"/repositories/{id}@{revision}/package.zip");

            return ZipPackage.FromStream(packageStream, stripFirstLevel: false);
        }

        public Task<RepositoryDetails> CreateAsync(RepositoryDetails repository)
        {
            return api.PostAsync<RepositoryDetails>($"/repositories", repository);
        }

        public Task<RepositoryDetails> UpdateAsync(RepositoryDetails repository)
        {
            return api.PatchAsync<RepositoryDetails>($"/repositories/{repository.Id}", repository);
        }
    }
}