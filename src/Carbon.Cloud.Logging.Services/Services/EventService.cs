using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Security;

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
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            
            return db.Events.QueryAsync(
                expression : Eq("resource", resource), 
                order      : Order.Descending("id"),
                skip       : 0,
                take       : 1000
            );
        }

        public Task<IReadOnlyList<Event>> ListAsync(IUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return db.Events.QueryAsync(Eq("userId", user.Id), Order.Descending("id"), 0, 1000);
        }
    }
}