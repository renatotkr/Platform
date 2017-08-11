using System;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Data.Sequences;

namespace Carbon.Platform
{
    public class EventClient
    {
        private readonly ApiBase api;

        internal EventClient(ApiBase api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public Task<EventDetails[]> ListAsync(Expression filter = null)
        {
            return api.GetListAsync<EventDetails>($"/events" + filter?.ToQueryString());
        }

        public Task<EventDetails> GetAsync(Uid id)
        {
            return api.GetAsync<EventDetails>($"/events/{id}");
        }
        
        public Task<EventDetails> CreateAsync(EventDetails record)
        {
            return api.PostAsync<EventDetails>($"/events", record);
        }
    }
}