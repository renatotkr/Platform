using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Apps;
using Carbon.Platform.Computing;
using Carbon.Versioning;

using Dapper;

namespace Carbon.Platform
{
    using static Expression;

    public class BackendService : IBackendService
    {
        private readonly PlatformDb db;

        public BackendService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }
        
        public async Task<BackendInfo> CreateAsync(
            string name, 
            IAppEnvironment environment,
            ManagedResource resource)
        {
            var backend = new BackendInfo(
                id          : db.Context.GetNextId<BackendInfo>(), 
                name        : name,
                env : environment,
                resource    : resource
            );

            await db.Backends.InsertAsync(backend).ConfigureAwait(false);

            return backend;
        }

        public async Task<IBackend> FindAsync(long id)
        {
            return await db.Backends.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IBackend> FindAsync(IAppEnvironment env, ILocation location)
        {
            var regionId = LocationId.Create(location.Id);

            // Ensure the location didn't provide a zone...
            regionId.Flags      = 0;
            regionId.ZoneNumber = 0;

            return await db.Backends.QueryFirstOrDefaultAsync(
                And(Eq("environmentId", env.Id), Eq("locationId", regionId.Value))
            ).ConfigureAwait(false);
        }

        public async Task UpdateEnvironmentRevision(IAppEnvironment env, SemanticVersion newVersion)
        {
            using (var connection = db.Context.GetConnection())
            {
                await connection.ExecuteAsync(
                    @"UPDATE AppEnvironments
                      SET revision = @newVersion
                      WHERE id = @id", new { id = env.Id, newVersion }
                 ).ConfigureAwait(false);
            }
        }

        public async Task<BackendInstance[]> GetInstancesAsync(IAppEnvironment env)
        {
            #region Preconditions

            if (env == null) throw new ArgumentNullException(nameof(env));

            #endregion

            var instances = await db.AppInstances.QueryAsync(
                And(Eq("environmentId", env.Id), IsNull("terminated"))
            ).ConfigureAwait(false);

            var hosts = new BackendInstance[instances.Count];

            for (var i = 0; i < instances.Count; i++)
            {
                var instance = instances[i];

                var host = await GetHostAsync(instance.HostId).ConfigureAwait(false);

                hosts[i] = new BackendInstance(host, instance.Port ?? 80);
            }

            return hosts;
        }

        public async Task<BackendInstance[]> GetInstancesAsync(long backendId)
        {
            var instances = await db.AppInstances.QueryAsync(
                And(Eq("backendId", backendId), IsNull("terminated"))
            ).ConfigureAwait(false);

            var hosts = new BackendInstance[instances.Count];

            for (var i = 0; i < instances.Count; i++)
            {
                var instance = instances[i];

                var host = await GetHostAsync(instance.HostId).ConfigureAwait(false);

                hosts[i] = new BackendInstance(host, instance.Port ?? 80);
            }

            return hosts;
        }

        private static readonly ConcurrentDictionary<long, HostInfo> hostCache =
            new ConcurrentDictionary<long, HostInfo>();

        private async Task<HostInfo> GetHostAsync(long id)
        {
            if (!hostCache.TryGetValue(id, out var host))
            {
                host = await db.Hosts.FindAsync(id).ConfigureAwait(false);

                if (host == null)
                {
                    throw new Exception($"host#{id} does not exist");
                }

                hostCache.TryAdd(id, host);
            }

            return host;
        }
    }
}

// A host may be servicing mutiple backends...
// Shoud we force each app to live in a container?