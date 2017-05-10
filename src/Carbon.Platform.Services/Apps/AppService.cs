using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Logs;
using Carbon.Platform.Resources;
using Carbon.Platform.Services;
using Carbon.Versioning;

using Dapper;

namespace Carbon.Platform.Apps
{
    using static Expression;

    public class AppService : IAppService
    {
        private readonly PlatformDb db;
        private readonly EnvironmentService envService;

        public AppService(PlatformDb db, IEnvironmentService envService)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.envService = (EnvironmentService)envService;
        }

        public async Task<AppInfo> GetAsync(long id)
        {
            return await db.Apps.FindAsync(id).ConfigureAwait(false) ?? throw new Exception($"app#{id} not found");
        }

        public Task<AppInfo> FindAsync(string name)
        {
            return db.Apps.QueryFirstOrDefaultAsync(Eq("name", name));
        }

        public Task<IReadOnlyList<AppInfo>> ListAsync(long ownerId)
        {
            return db.Apps.QueryAsync(
                And(Eq("ownerId", ownerId), IsNull("deleted")),
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

            // this sequence incriments by 4, reserving 4 consecutive ids
            var id = db.Apps.IdGenerator.Next(); 

            var app = new AppInfo(
                id      : id,
                name    : name,
                ownerId : ownerId
            );

            // Each app is given 4 environments (using the consecutive ids reserved above): 
            // Production, Staging, Intergration, and Development

            foreach (var envType in new[] {
                EnvironmentType.Production,   // 1
                EnvironmentType.Staging,      // 2
                EnvironmentType.Intergration, // 3
                EnvironmentType.Development   // 4
            })
            {
                await CreateEnvironmentAsync(app, envType).ConfigureAwait(false);
            }

            await db.Apps.InsertAsync(app).ConfigureAwait(false);

            return app;
        }

        internal async Task<EnvironmentInfo> CreateEnvironmentAsync(IApp app, EnvironmentType type)
        {
            // Production   = appId
            // Staging      = appId + 1
            // Intergration = appId + 2
            // Development  = appId + 3

            var envIdOffset = ((int)type) - 1;

            var env = new EnvironmentInfo(
                id    : app.Id + envIdOffset,
                appId : app.Id,
                type  : type
            );

            await db.Environments.InsertAsync(env).ConfigureAwait(false);

            return env;
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
            {
                return (await connection.ExecuteScalarAsync<int>(
                    @"SELECT `releaseCount` FROM `Apps` WHERE id = @id FOR UPDATE;
                      UPDATE `Apps`
                      SET `releaseCount` = `releaseCount` + 1
                      WHERE id = @id", new { id = app.Id }).ConfigureAwait(false)) + 1;
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

        public Task<EnvironmentInfo> GetEnvironmentAsync(long appId, EnvironmentType type)
        {
            return envService.GetAsync(new A(appId), type);
        }

        public Task<IReadOnlyList<EnvironmentInfo>> GetEnvironmentsAsync(IApp app)
        {
            return db.Environments.QueryAsync(Eq("appId", app.Id));
        }

        class A : IApp
        {
            internal A(long id)
            {
                Id = id;
            }
            public long Id { get; }

            public string Name => "";
        }
    }

    
}