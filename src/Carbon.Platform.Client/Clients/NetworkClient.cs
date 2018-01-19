using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Networking;

namespace Carbon.Platform
{
    public class NetworkClient
    {
        private readonly ApiBase api;

        internal NetworkClient(ApiBase api)
        {
            this.api = api;
        }

        public Task<NetworkDetails[]> ListAsync(Expression filter = null)
        {
            return api.GetListAsync<NetworkDetails>($"/networks" + filter?.ToQueryString());
        }

        public Task<NetworkDetails> GetAsync(long id)
        {
            Ensure.IsValidId(id);

            return api.GetAsync<NetworkDetails>($"/networks/{id}");
        }
    }
}