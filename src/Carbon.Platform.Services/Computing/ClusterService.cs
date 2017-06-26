using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Environments;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    using static Expression;

    public class ClusterService : IClusterService
    {
        private readonly PlatformDb db;

        public ClusterService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<Cluster>> ListAsync(IEnvironment env)
        {
            return db.Clusters.QueryAsync(Eq("environmentId", env.Id));
        }

        public async Task<Cluster> CreateAsync(CreateClusterRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var cluster = new Cluster(
               id             : db.Clusters.Sequence.Next(),
               name           : request.Name,
               environmentId  : request.EnvironmentId,
               locationId     : request.LocationId,
               properties     : request.Properties,
               healthCheckId  : request.HealthCheckId,
               hostTemplateId : request.HostTemplateId
            );

            await db.Clusters.InsertAsync(cluster).ConfigureAwait(false);

            return cluster;
        }

        public async Task<Cluster> GetAsync(long id)
        {
            return await db.Clusters.FindAsync(id).ConfigureAwait(false)
                ?? throw ResourceError.NotFound(ResourceTypes.Cluster, id);
        }

        public async Task<Cluster> GetAsync(IEnvironment env, ILocation location)
        {
            var group = await db.Clusters.QueryFirstOrDefaultAsync(
                Conjunction(
                    Eq("environmentId", env.Id),
                    Eq("locationId", location.Id),
                    IsNull("deleted")
                )
            ).ConfigureAwait(false);

            if (group == null)
            {
                throw new ResourceNotFoundException($"cluster(env#{env.Id}, location#{location.Id})");
            }
            
            return group;
        }
    }
}