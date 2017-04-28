using System;
using System.Linq;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Apps;
using Carbon.Platform.Computing;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Services
{
    using static Expression;

    public class EnvironmentService : IEnvironmentService
    {
        private readonly PlatformDb db;

        public EnvironmentService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<EnvironmentInfo> GetAsync(IApp app, EnvironmentType type)
        {
            return db.Environments.QueryFirstOrDefaultAsync(
                And(Eq("appId", app.Id), Eq("type", type)
            ));
        }

        public async Task<EnvironmentResource> AddResourceAsync(IEnvironment env, ILocation location, IResource resource)
        {
            var id = await db.EnvironmentResources.GetNextScopedIdAsync(env.Id).ConfigureAwait(false);

            var envResource = new EnvironmentResource(
                id          : id,
                environment : env,
                location    : location,
                resource    : resource
            );
            
            await db.EnvironmentResources.InsertAsync(envResource).ConfigureAwait(false);

            return envResource;
        }

        public Task<EnvironmentInfo> CreateAsync(IApp app, EnvironmentType type)
        {
            return CreateAsync(app, type, Array.Empty<ILocation>());
        }
     
        public async Task<EnvironmentInfo> CreateAsync(IApp app, EnvironmentType type, ILocation[] regions)
        {
            #region Preconditions

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (regions == null)
                throw new ArgumentNullException(nameof(regions));

            #endregion

            var env = new EnvironmentInfo(
                id    : db.Context.GetNextId<EnvironmentInfo>(),
                appId : app.Id,
                type  : type
            );

            // Create the regions
            foreach (var region in regions)
            {
                var envLocation = new EnvironmentLocation(env.Id, region.Id);

                var group = new HostGroup(
                    id       : await db.HostGroups.GetNextScopedIdAsync(env.Id).ConfigureAwait(false),
                    name     : app.Name, 
                    resource : ManagedResource.HostGroup(region, Guid.NewGuid().ToString())
                );

                await db.HostGroups.InsertAsync(group).ConfigureAwait(false);
                await db.EnvironmentLocations.InsertAsync(envLocation).ConfigureAwait(false);
            }

            await db.Environments.InsertAsync(env).ConfigureAwait(false);

            return env;
        }

        public async Task<IHost[]> GetHostsAsync(IEnvironment env, ILocation location)
        {
            var locationId = LocationId.Create(location.Id);

            var idStart = HostId.Create(locationId.WithZoneNumber(0),             sequenceNumber: 0);
            var idEnd   = HostId.Create(locationId.WithZoneNumber(byte.MaxValue), sequenceNumber: int.MaxValue);

            var hosts = await db.Hosts.QueryAsync(
                Conjunction(
                    Between("id", idStart, idEnd),
                    Eq("environmentId", env.Id),
                    IsNull("terminated")
                )
            ).ConfigureAwait(false);

            return hosts.ToArray();
        }

        public async Task<IHost[]> GetHostsAsync(IEnvironment env)
        {
            #region Preconditions

            if (env == null) throw new ArgumentNullException(nameof(env));

            #endregion

            // An environment may be divided into x groups

            var instances = await db.Hosts.QueryAsync(
                And(Eq("environmentId", env.Id), IsNull("terminated"))
            ).ConfigureAwait(false);

            return instances.ToArray();
        }
    }
}