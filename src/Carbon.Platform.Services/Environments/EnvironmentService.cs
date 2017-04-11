using System;
using System.Linq;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Apps;
using Carbon.Platform.Computing;

namespace Carbon.Platform
{
    using static Expression;

    public class EnvironmentService : IEnvironmentService
    {
        private readonly PlatformDb db;

        public EnvironmentService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        // Cache?

        public Task<AppEnvironment> GetAsync(long appId, string name)
        {
            return db.Environments.QueryFirstOrDefaultAsync(And(Eq("appId", appId), Eq("name", name)));
        }
   
        public async Task<AppEnvironment> CreateAsync(IApp app, string name, ILocation[] regions)
        {
            #region Preconditions

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (regions == null)
                throw new ArgumentNullException(nameof(regions));

            #endregion

            var env = new AppEnvironment(
                id    : db.Context.GetNextId<AppEnvironment>(),
                appId : app.Id,
                name  : name
            );

            // Create the regions
            foreach (var region in regions)
            {
                var envLocation = new EnvironmentLocation(env.Id, region.Id);

                var group = new HostGroup(
                    id       : await db.HostGroups.GetNextScopedIdAsync(env.Id).ConfigureAwait(false),
                    name     : app.Name, 
                    resource : new ManagedResource(region, ResourceType.HostGroup, Guid.NewGuid().ToString())
                );

                await db.HostGroups.InsertAsync(group).ConfigureAwait(false);
                await db.EnvironmentLocations.InsertAsync(envLocation).ConfigureAwait(false);
            }

            await db.Environments.InsertAsync(env).ConfigureAwait(false);

            return env;
        }

        public async Task<IHost[]> GetHostsAsync(IEnvironment env, ILocation location)
        {
            var idStart = HostId.Create(LocationId.Create(location.Id).WithZoneNumber(0), 0);
            var idEnd   = HostId.Create(LocationId.Create(location.Id).WithZoneNumber(byte.MaxValue), int.MaxValue);

            var hosts = await db.Hosts.QueryAsync(
                And(Between("id", idStart, idEnd), Eq("environmentId", env.Id))
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