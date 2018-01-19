using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Computing;
using Carbon.Platform.Environments;
using Carbon.Platform.Resources;

namespace Carbon.Platform
{
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

        public Task<HostDetails[]> ListAsync(ICluster cluster)
        {
            Ensure.NotNull(cluster, nameof(cluster));

            return api.GetListAsync<HostDetails>($"/clusters/{cluster.Id}/hosts");
        }

        public Task<HostDetails[]> ListAsync(IEnvironment environment)
        {
            Ensure.NotNull(environment, nameof(environment));

            return api.GetListAsync<HostDetails>($"/environments/{environment.Id}/hosts");
        }

        public Task<HostDetails> GetAsync(long id)
        {
            Ensure.IsValidId(id);

            return api.GetAsync<HostDetails>($"/hosts/{id}");
        }

        public Task<HostDetails> GetAsync(ManagedResource resource)
        {
            var provider = ResourceProvider.Get(resource.ProviderId);

            return api.GetAsync<HostDetails>($"/hosts/{provider.Code}:{resource.ResourceId}");
        }
    }
}