using System;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Environments;

namespace Carbon.Platform
{
    public class UserClient
    {
        private readonly ApiBase api;

        internal UserClient(ApiBase api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public Task<UserDetails[]> ListAsync(Expression filter = null)
        {
            return api.GetListAsync<UserDetails>($"/users" + filter?.ToQueryString());
        }

        public Task<UserDetails[]> ListAsync(IEnvironment environment)
        {
            Ensure.NotNull(environment, nameof(environment));

            return api.GetListAsync<UserDetails>($"/environments/{environment.Id}/users");
        }
        
        public Task<UserDetails> GetAsync(long id)
        {
            Ensure.IsValidId(id);

            return api.GetAsync<UserDetails>($"/users/{id}");
        }

        public Task<UserDetails> CreateAsync(UserDetails user)
        {
            return api.PostAsync<UserDetails>($"/users", user);
        }
    }
}