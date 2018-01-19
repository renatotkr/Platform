using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Computing;

namespace Carbon.Platform
{
    public class ImageClient
    {
        private readonly ApiBase api;

        internal ImageClient(ApiBase api)
        {
            this.api = api;
        }

        public Task<ImageDetails[]> ListAsync(Expression filter = null)
        {
            return api.GetListAsync<ImageDetails>($"/images" + filter?.ToQueryString());
        }

        public Task<ImageDetails> GetAsync(long id)
        {
            Ensure.IsValidId(id);

            return api.GetAsync<ImageDetails>($"/images/{id}");
        }
        
        // TODO: Create
    }
}