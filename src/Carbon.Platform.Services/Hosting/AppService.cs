using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Protection;
using Carbon.Versioning;

namespace Carbon.Platform.Apps
{
    using static Expression;

    public class AppService
    {
        private readonly PlatformDb db;

        public AppService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<AppInfo> Get(long id)
        {
            return db.Apps.FindAsync(id);
        }

        public Task<AppInfo> Find(string name)
        {
            return db.Apps.QueryFirstOrDefaultAsync(Eq("name", name));
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
                id      : db.Context.GetNextId<AppInfo>(),
                name    : request.Name,
                type    : request.Type,
                ownerId : request.OwnerId
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

            return release;
        }

        public Task<IReadOnlyList<AppEnvironment>> GetEnvironments(IApp app)
        {
            return db.Environments.QueryAsync(Eq("appId", app.Id));
        }
    }


    public class CreateAppRequest
    {
        public string Name { get; set; }

        public AppType Type { get; set; }

        public long OwnerId { get; set; }
    }
}