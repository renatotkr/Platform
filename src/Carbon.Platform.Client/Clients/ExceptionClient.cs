using System;
using System.Threading.Tasks;

using Carbon.Platform.Diagnostics;
using Carbon.Platform.Environments;

namespace Carbon.Platform
{
    public class ExceptionClient
    {
        private readonly ApiBase api;

        internal ExceptionClient(ApiBase api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public Task<ExceptionDetails[]> ListAsync(IEnvironment environment)
        {
            return api.GetListAsync<ExceptionDetails>($"/environments/{environment.Id}/exceptions");
        }

        // By host?
        
        public Task<ExceptionDetails> GetAsync(long id)
        {
            return api.GetAsync<ExceptionDetails>($"/exceptions/{id}");
        }

        public Task<ExceptionDetails> CreateAsync(ExceptionDetails exception)
        {
            return api.PostAsync<ExceptionDetails>(
                path : $"/exceptions",
                data : exception
            );
        }
    }
}