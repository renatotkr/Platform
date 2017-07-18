using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Platform.Computing;

namespace Carbon.Platform
{
    using Resources;

    public class ClusterClient
    {
        private readonly ApiBase api;

        internal ClusterClient(ApiBase api)
        {
            this.api = api;
        }
        
        // list async?

        public Task<ClusterDetails> GetAsync(long id)
        {
            return api.GetAsync<ClusterDetails>($"/clusters/{id}");
        }
    }
}