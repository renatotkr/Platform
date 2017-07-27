using System;
using System.Threading.Tasks;

using Carbon.Data.Expressions;

namespace Carbon.Platform
{
    public class EnvironmentClient
    {
        private readonly ApiBase api;

        internal EnvironmentClient(ApiBase api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public Task<EnvironmentDetails[]> ListAsync(Expression filter = null)
        {
            return api.GetListAsync<EnvironmentDetails>("/environments" + filter?.ToQueryString());
        }

        public Task<EnvironmentDetails> GetAsync(long id)
        {
            return api.GetAsync<EnvironmentDetails>($"/environments/{id}");
        }

        public Task<EnvironmentDetails> CreateAsync(EnvironmentDetails record)
        {
            return api.PostAsync<EnvironmentDetails>(
                path : $"/environments",
                data : record
            );
        }

        public Task<EnvironmentDetails> UpdateAsync(EnvironmentDetails record)
        {
            return api.PatchAsync<EnvironmentDetails>(
                path : $"/environments/" + record.Id,
                data : record
            );
        }
    }
}