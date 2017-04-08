using System;
using System.Threading.Tasks;

using Carbon.Json;
using Carbon.Protection;
using Carbon.Versioning;

namespace Carbon.Platform.Apps
{
    public class AppService
    {
        private readonly PlatformDb db;

        public AppService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<AppInfo> CreateAsync(string name, AppType type, long ownerId)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (AppName.Validate(name) == false)
                throw new ArgumentException("invalid", nameof(name));

            #endregion

            var app = new AppInfo(
                id: db.Context.GetNextId<AppInfo>(),
                name: name,
                type: type
            )
            {
                OwnerId = ownerId
            };

            await db.Apps.InsertAsync(app).ConfigureAwait(false);

            return app;
        }

        public async Task<AppRelease> CreateReleaseAsync(IApp app, SemanticVersion version, Hash digest, long creatorId)
        {
            var release = new AppRelease(app, version, digest) { CreatorId = creatorId };
            
            await db.AppReleases.InsertAsync(release).ConfigureAwait(false);

            return release;
        }

        public async Task<AppEnvironment> CreateEnvironmentAsync(IApp app, string name)
        {
            var resource = new ManagedResource(ResourceProvider.Borg, ResourceType.AppEnvironment, Guid.NewGuid().ToString());

            var env = new AppEnvironment(
                id        : await db.AppEnvironments.GetNextScopedIdAsync(app.Id).ConfigureAwait(false),
                name      : name,
                variables : new JsonObject(),
                resource  : resource
            );
            
            await db.AppEnvironments.InsertAsync(env).ConfigureAwait(false);

            return env;
        }
    }
}