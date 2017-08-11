using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Cloud.Logging
{
    public interface IEventService
    {
        Task<IReadOnlyList<Event>> ListHavingUserIdAsync(long userId);

        Task<IReadOnlyList<Event>> ListHavingResourceAsync(string resource);
    }
}