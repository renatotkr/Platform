using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Computing;

namespace Carbon.Platform
{
    public class ClusterClient
    {
        private readonly ApiBase api;

        internal ClusterClient(ApiBase api)
        {
            this.api = api;
        }

        public Task<ClusterDetails[]> ListAsync(Expression filter = null)
        {
            return api.GetListAsync<ClusterDetails>($"/clusters" + filter?.ToQueryString());
        }

        public Task<ClusterDetails> GetAsync(long id)
        {
            Ensure.IsValidId(id);

            return api.GetAsync<ClusterDetails>($"/clusters/{id}");
        }

        public Task<ClusterDetails> CreateAsync(ClusterDetails cluster)
        {
            Ensure.NotNull(cluster, nameof(cluster));

            return api.PostAsync<ClusterDetails>($"/clusters", cluster);
        }
    }
}