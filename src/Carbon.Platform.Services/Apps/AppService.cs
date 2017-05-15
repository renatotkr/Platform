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
        public async Task<AppInfo> CreateAsync(CreateAppRequest request)
        {
            #region Preconditions

            if (request.Name == null)
                throw new ArgumentNullException(nameof(request.Name));

            if (AppName.Validate(request.Name) == false)
                throw new ArgumentException($"Invalid name '{request.Name}", nameof(request.Name));
            
            if (request.OwnerId <= 0)
                throw new ArgumentException("Must be greater than 0", nameof(request.OwnerId));

            #endregion

            // this sequence incriments by 4, reserving 4 consecutive ids
            var id = db.Apps.Sequence.Next(); 

            var app = new AppInfo(
                id      : id,
                name    : request.Name,
                ownerId : request.OwnerId
            );

            // Each app is given 4 environments (using the consecutive ids reserved above): 
            // Production, Staging, Intergration, and Development

            var environments = new[] {
                GetConfiguredEnvironment(app, EnvironmentType.Production),   // 1
                GetConfiguredEnvironment(app, EnvironmentType.Staging),      // 2
                GetConfiguredEnvironment(app, EnvironmentType.Intergration), // 3
                GetConfiguredEnvironment(app, EnvironmentType.Development),  // 4
            };
            
            await db.Apps.InsertAsync(app).ConfigureAwait(false);
            
            await db.Environments.InsertAsync(environments).ConfigureAwait(false);

            return app;
        }

        private EnvironmentInfo GetConfiguredEnvironment(IApp app, EnvironmentType type)
        {
            // Production   = appId
            // Staging      = appId + 1
            // Intergration = appId + 2
            // Development  = appId + 3

            var envIdOffset = ((int)type) - 1;

            return new EnvironmentInfo(
                id   : app.Id + envIdOffset,
                name : app.Name + ((type == EnvironmentType.Production) ? "" : "#" + type.ToString().ToLower())
            );
        }

        private async Task<EnvironmentInfo> CreateEnvironmentAsync(IApp app, EnvironmentType type)
        {
            var env = GetConfiguredEnvironment(app, type);

            await db.Environments.InsertAsync(env).ConfigureAwait(false);

            return env;
        }

        public async Task<AppRelease> CreateReleaseAsync(CreateAppReleaseRequest request)
        {
            #region Preconditions

            Validate.Object(request, nameof(request));
            
            #endregion

            var release = new AppRelease(
                id        : await GetNextReleaseIdAsync(request.App), 
                appId     : request.App.Id,
                version   : request.Version, 
                sha256    : request.Sha256,
                creatorId : request.CreatorId
            );

            await db.AppReleases.InsertAsync(release).ConfigureAwait(false);

            var e = new Activity(ActivityType.Publish, request.App as IResource);

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
            return db.AppReleases.QueryFirstOrDefaultAsync(
                And(Eq("appId", appId), Eq("version", version))
            ); 
        }

        public Task<IReadOnlyList<AppRelease>> GetReleasesAsync(long appId)
        {
            return db.AppReleases.QueryAsync(Eq("appId", appId), Order.Descending("version"));
        }

        public Task<EnvironmentInfo> GetEnvironmentAsync(long appId, EnvironmentType type)
        {
            return envService.GetAsync(new A(appId), type);
        }

        public Task<IReadOnlyList<EnvironmentInfo>> GetEnvironmentsAsync(IApp app)
        {
            var appId = app.Id;
            
            // + 0 production
            // + 1 staging
            // + 2 intergration
            // + 3 development

            return db.Environments.QueryAsync(Between("id", app.Id, app.Id + 3));
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