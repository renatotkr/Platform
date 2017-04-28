using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Logs;
using Carbon.Platform.Resources;
using Carbon.Versioning;

using Dapper;

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
            return await db.Apps.FindAsync(id).ConfigureAwait(false) ?? throw new Exception($"app#{id} not found");
        }

        public Task<AppInfo> FindAsync(string name)
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

        // TODO: Create the environments
        public async Task<AppInfo> CreateAsync(string name, long ownerId)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (AppName.Validate(name) == false)
                throw new ArgumentException("invalid", nameof(name));
            
            if (ownerId <= 0)
                throw new ArgumentException("Must be greater than 0", nameof(ownerId));

            #endregion

            // Reserve 3 ids (and create environments?)
            var app = new AppInfo(
                id      : db.Context.GetNextId<AppInfo>(),
                name    : name,
                ownerId : ownerId
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

            var releaseId = await GetNextReleaseIdAsync(app);

            var release = new AppRelease(releaseId, app, version, sha256, creatorId);

            await db.AppReleases.InsertAsync(release).ConfigureAwait(false);

            var e = new Activity(ActivityType.Publish, app as IResource);

            await db.Activities.InsertAsync(e);

            return release;
        }

        private async Task<long> GetNextReleaseIdAsync(IApp app)
        {
            using (var connection = db.Context.GetConnection())
            using (var ts = connection.BeginTransaction())
            {
                var result = await connection.ExecuteScalarAsync<int>(
                    @"SELECT `releaseCount` FROM `Apps` WHERE id = @id FOR UPDATE;
                      UPDATE `Apps`
                      SET `releaseCount` = `releaseCount` + 1
                      WHERE id = @id", app, ts).ConfigureAwait(false);

                ts.Commit();

                return result + 1;
            }
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

        public Task<EnvironmentInfo> GetEnvironmentAsync(long appId, string name)
        {
            return db.Environments.QueryFirstOrDefaultAsync(
                And(Eq("appId", appId), Eq("name", name))
            );
        }

        public Task<IReadOnlyList<EnvironmentInfo>> GetEnvironmentsAsync(IApp app)
        {
            return db.Environments.QueryAsync(Eq("appId", app.Id));
        }
    }
}