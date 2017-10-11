using System;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Computing;

namespace Carbon.Platform
{
    public class DomainClient
    {
        private readonly ApiBase api;

        internal DomainClient(ApiBase api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public Task<DomainDetails[]> ListAsync(Expression filter = null)
        {
            return api.GetListAsync<DomainDetails>($"/domains" + filter?.ToQueryString());
        }

        // List by owner

        public Task<ProgramDetails> GetAsync(long id)
        {
            return api.GetAsync<ProgramDetails>($"/domains/{id}");
        }

        public Task<ProgramDetails> CreateAsync(DomainDetails domain)
        {
            return api.PostAsync<ProgramDetails>(
                path : $"/programs",
                data : domain
            );
        }
    }
}