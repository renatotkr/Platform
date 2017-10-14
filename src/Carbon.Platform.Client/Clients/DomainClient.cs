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

        // ?ownerId=1
        public Task<DomainDetails[]> ListAsync(Expression filter = null)
        {
            return api.GetListAsync<DomainDetails>($"/domains" + filter?.ToQueryString());
        }

        public Task<DomainDetails> GetAsync(long id)
        {
            return api.GetAsync<DomainDetails>($"/domains/{id}");
        }

        public Task<DomainDetails> GetAsync(string name)
        {
            return api.GetAsync<DomainDetails>($"/domains/{name}");
        }

        public Task<DomainDetails> CreateAsync(DomainDetails domain)
        {
            #region Preconditions

            if (domain == null)
                throw new ArgumentNullException(nameof(domain));

            if (domain.Name == null)
                throw new ArgumentNullException(nameof(domain.Name));

            #endregion

            return api.PostAsync<DomainDetails>(
                path : $"/domains",
                data : domain
            );
        }
    }
}