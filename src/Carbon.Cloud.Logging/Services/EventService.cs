using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;

namespace Carbon.Cloud.Logging
{
    using static Expression;

    public class EventService : IEventService
    {
        private readonly LogsDb db;

        public EventService(LogsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        // e.g. host#500
        public Task<IReadOnlyList<Event>> ListHavingResourceAsync(string resource)
        {
            #region Preconditions

            if (resource == null)
                throw new ArgumentNullException(nameof(resource));

            #endregion

            return db.Events.QueryAsync(
                expression : Eq("resource", resource), 
                order      : Order.Descending("id"),
                skip       : 0,
                take       : 1000
            );
        }

        public Task<IReadOnlyList<Event>> ListHavingUserIdAsync(long userId)
        {
            return db.Events.QueryAsync(Eq("userId", userId), Order.Descending("id"), 0, 1000);
        }
    }
}