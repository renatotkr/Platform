using System;
using System.IO;
using System.Threading.Tasks;

using Carbon.Packaging;
using Carbon.Platform.Computing;
using Carbon.Storage;
using Carbon.Versioning;
using Carbon.Platform.Storage;

namespace Carbon.Platform
{
    public class RepositoryClient
    {
        private readonly ApiBase api;

        internal RepositoryClient(ApiBase api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public Task<RepositoryDetails> GetAsync(long id)
        {
            return api.GetAsync<RepositoryDetails>($"/repositories/{id}");
        }

    }
}