using System;
using System.Threading.Tasks;

namespace Carbon.Cloud.Logging
{
    public class EventLogger : IEventLogger
    {
        private readonly LogsDb db;

        public EventLogger(LogsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task CreateAsync(Event @event)
        {
            #region Preconditions

            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            #endregion

            if (@event.Id.Lower == 0 || @event.Id.Upper == 0)
            {
                @event.Id = RequestId.Create(1, DateTime.UtcNow, 0);
            }
            
            await db.Events.InsertAsync(@event);
        }

        public async Task CreateAsync(Event[] @events)
        {
            #region Preconditions

            if (@events == null)
                throw new ArgumentNullException(nameof(@events));

            #endregion

            await db.Events.InsertAsync(@events);
        }
    }
}