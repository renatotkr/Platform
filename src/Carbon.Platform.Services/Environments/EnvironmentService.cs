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

        public Task<EnvironmentInfo> GetAsync(long id)
        {
            return db.Environments.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceType.Environment, id);
        }

        public Task<EnvironmentInfo> GetAsync(IApp app, EnvironmentType type)
        {
            // We can lookup directly by id...

            var id = app.Id + (((int)type) - 1);

            return GetAsync(id);
        }

        public async Task<EnvironmentResource> AddResourceAsync(
            IEnvironment env, 
            ILocation location, 
            IResource resource)
        {
            var id = await db.EnvironmentResources.GetNextScopedIdAsync(env.Id).ConfigureAwait(false);

            var envResource = new EnvironmentResource(
                id       : id,
                env      : env,
                location : location,
                resource : resource
            );
            
            await db.EnvironmentResources.InsertAsync(envResource).ConfigureAwait(false);

            return envResource;
        }
        
        public async Task AddLocations(IEnvironment env, ILocation[] locations)
        {
            var records = new EnvironmentLocation[locations.Length];

            for (var i = 0; i < locations.Length; i++)
            {
                records[i] = new EnvironmentLocation(env.Id, locations[i].Id);
            }

            await db.EnvironmentLocations.InsertAsync(records).ConfigureAwait(false);
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

            if (env == null)
                throw new ArgumentNullException(nameof(env));

            #endregion

            // An environment may be divided into x groups

            var instances = await db.Hosts.QueryAsync(
                And(Eq("environmentId", env.Id), IsNull("terminated"))
            ).ConfigureAwait(false);

            return instances.ToArray();
        }
    }
}