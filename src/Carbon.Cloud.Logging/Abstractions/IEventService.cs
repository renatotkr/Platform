using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Security;

namespace Carbon.Cloud.Logging
{
    public interface IEventService
    {
        Task<IReadOnlyList<Event>> ListAsync(IUser user);

        Task<IReadOnlyList<Event>> ListHavingResourceAsync(string resource);
    }
}