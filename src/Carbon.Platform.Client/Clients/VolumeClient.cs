using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Storage;

namespace Carbon.Platform
{
    public class VolumeClient
    {
        private readonly ApiBase api;

        internal VolumeClient(ApiBase api)
        {
            this.api = api;
        }

        public Task<VolumeDetails[]> ListAsync(Expression filter = null)
        {
            return api.GetListAsync<VolumeDetails>($"/volumes" + filter?.ToQueryString());
        }

        public Task<VolumeDetails> GetAsync(long id)
        {
            Ensure.IsValidId(id);

            return api.GetAsync<VolumeDetails>($"/volumes/{id}");
        }
    }
}