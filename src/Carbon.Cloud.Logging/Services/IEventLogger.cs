using System.Threading.Tasks;

namespace Carbon.Cloud.Logging
{
    public interface IEventLogger
    {
        Task CreateAsync(Event @event);
    }
}