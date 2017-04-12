using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Logs;
using Carbon.Platform.Resources;
using Carbon.Versioning;

namespace Carbon.Platform.Apps
{
    using static Expression;

    public class AppService : IAppService
    {
        private readonly PlatformDb db;

        public AppService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<AppInfo> GetAsync(long id)
        {
            var app = await db.Apps.FindAsync(id);

            if (app == null) throw new Exception($"app#{id} not found");
            return app;
        }

        public Task<AppInfo> GetAsync(string name)
        {
            return db.Apps.QueryFirstOrDefaultAsync(Eq("name", name));
        }

        public Task<IReadOnlyList<AppInfo>> ListAsync()
        {
            return db.Apps.QueryAsync(
                IsNull("deleted"),
                Order.Ascending("name")
             );
        }

        public async Task<AppInfo> CreateAsync(CreateAppRequest request)
        {
            #region Preconditions

            if (request.Name == null)
                throw new ArgumentNullException(nameof(request.Name));

            if (AppName.Validate(request.Name) == false)
                throw new ArgumentException("invalid", nameof(request.Name));

            #endregion

            var app = new AppInfo(
                id   : db.Context.GetNextId<AppInfo>(),
                name : request.Name,
                type : request.Type
            );

            await db.Apps.InsertAsync(app).ConfigureAwait(false);

            return app;
        }

        public async Task<AppRelease> CreateReleaseAsync(
            IApp app, 
            SemanticVersion version, 
            byte[] sha256,
            long creatorId)
        {
            #region Preconditions

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (sha256 == null)
                throw new ArgumentNullException(nameof(sha256));

            if (sha256.Length != 32)
                throw new ArgumentException("Invalid hash", nameof(sha256));

            #endregion

            var release = new AppRelease(app, version, sha256, creatorId);

            await db.AppReleases.InsertAsync(release).ConfigureAwait(false);

            var e = new Activity(ActivityType.Publish, app as IResource);

            await db.Activities.InsertAsync(e);

            return release;
        }

        public Task<AppRelease> GetReleaseAsync(long appId, SemanticVersion version)
        {
            return db.AppReleases.FindAsync((appId, version));
        }

        public Task<IReadOnlyList<AppRelease>> GetReleasesAsync(long appId)
        {
            return db.AppReleases.QueryAsync(
                 Eq("appId", appId),
                 Order.Descending("version")
             );
        }

        public Task<AppEnvironment> GetEnvironmentAsync(long appId, string name)
        {
            return db.Environments.QueryFirstOrDefaultAsync(
                And(Eq("appId", appId), Eq("name", name))
            );
        }

        public Task<IReadOnlyList<AppEnvironment>> GetEnvironmentsAsync(IApp app)
        {
            return db.Environments.QueryAsync(Eq("appId", app.Id));
        }
    }


    public class CreateAppRequest
    {
        public string Name { get; set; }

        public AppType Type { get; set; }
    }
}