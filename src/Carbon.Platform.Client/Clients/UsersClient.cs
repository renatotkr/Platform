using System;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Environments;

namespace Carbon.Platform
{
    public class UsersClient
    {
        private readonly ApiBase api;

        internal UsersClient(ApiBase api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public Task<UserDetails[]> ListAsync(Expression filter = null)
        {
            return api.GetListAsync<UserDetails>($"/users" + filter?.ToQueryString());
        }

        public Task<UserDetails[]> ListAsync(IEnvironment environment)
        {
            return api.GetListAsync<UserDetails>($"/environments/{environment.Id}/users");
        }
        
        public Task<UserDetails> GetAsync(long id)
        {
            return api.GetAsync<UserDetails>($"/users/{id}");
        }

        public Task<UserDetails> CreateAsync(UserDetails user)
        {
            return api.PostAsync<UserDetails>(
                path : $"/users",
                data : user
            );
        }
    }
}