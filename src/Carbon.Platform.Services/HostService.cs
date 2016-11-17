using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Hosting
{
    using Computing;
    using Data;
    using Platform;

    using static Data.Expression;

    public class HostService
    {
        private readonly PlatformDb db;
        
        public HostService(PlatformDb db)
        {
            this.db = db;
        }
        
        public Task<Host> FindHavingInstanceIdAsync(string instanceId)
            => db.Hosts.QueryFirstOrDefaultAsync(Eq("instanceId", instanceId));

        public Task<IList<Host>> FindAllAsync(long[] ids)
            => db.Hosts.QueryAsync(In("Id", ids));
        
        // TODO: Cache

        public Task<Host> FindAsync(long id) 
            => db.Hosts.GetAsync(new Key<Host>(id));
    }
}
