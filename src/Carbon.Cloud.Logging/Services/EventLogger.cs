using System.Threading.Tasks;

namespace Carbon.Cloud.Logging
{
    public class EventLogger : IEventLogger
    {
        private readonly LoggingDb db;

        public EventLogger(LoggingDb db)
        {
            this.db = db;
        }

        public async Task CreateAsync(Event @event)
        {
            await db.Events.InsertAsync(@event);
        }

        public async Task CreateAsync(Event[] @events)
        {
            await db.Events.InsertAsync(@events);
        }
    }
}