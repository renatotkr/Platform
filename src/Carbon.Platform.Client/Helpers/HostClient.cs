using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Platform.Computing;

namespace Carbon.Platform
{
    using Resources;

    public class HostClient
    {
        private readonly ApiBase api;

        internal HostClient(ApiBase api)
        {
            this.api = api;
        }

        // hosts/aws:i-0234123
        public Task<HostDetails> RegisterAsync(HostDetails host)
        {
            var provider = ResourceProvider.Get(host.Resource.ProviderId);

            return api.PostAsync<HostDetails>(
                $"/hosts/{provider.Code}:{host.Resource.ResourceId}", host);
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

        public Task<List<ProgramDetails>> ListProgramsAsync(long id)
        {
            return api.GetAsync<List<ProgramDetails>>($"/hosts/{id}/programs");
        }
    }
}