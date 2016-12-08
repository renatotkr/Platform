using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Services
{
    using Apps;
    using Versioning;  

    using static Data.Expressions.Expression;

    public class AppService
    {
        private readonly PlatformDb db;

        public AppService(PlatformDb db)
        {
            #region Preconditions

            if (db == null) throw new ArgumentNullException(nameof(db));

            #endregion

            this.db = db;
        }

        // Update Hearbeat

        public Task<App> GetAsync(long id)
            => db.Apps.FindAsync(id);
        
        public Task<IList<AppInstance>> GetInstancesAsync(long id)
            => db.AppInstances.QueryAsync(Eq("appId", id));

        public Task<IList<AppEvent>> GetEventsAsync(long id)
            => db.AppEvents.QueryAsync(Eq("appId", id));

        public Task<IList<AppRelease>> GetReleasesAsync(long id)
           => db.AppReleases.QueryAsync(Eq("appId", id));

        public Task<AppRelease> GetReleaseAsync(long id, SemanticVersion version)
            => db.AppReleases.QueryFirstOrDefaultAsync(
                Conjunction(Eq("appId", id), Eq("version", version))
            );

    }
}