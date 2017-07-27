using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Platform.Computing;

namespace Carbon.Platform
{
    using System.Text;
    using Carbon.Data.Expressions;
    using Resources;

    public class HostClient
    {
        private readonly ApiBase api;

        internal HostClient(ApiBase api)
        {
            this.api = api;
        }

        public Task<HostDetails[]> ListAsync(Expression filter = null)
        {
            return api.GetListAsync<HostDetails>($"/hosts" + filter?.ToQueryString());
        }

        public Task<HostDetails> GetAsync(long id)
        {
            return api.GetAsync<HostDetails>($"/hosts/{id}");
        }

        public Task<HostDetails> GetAsync(ManagedResource resource)
        {
            var provider = ResourceProvider.Get(resource.ProviderId);

            return api.GetAsync<HostDetails>($"/hosts/{provider.Code}:{resource.ResourceId}");
        }
    }
}