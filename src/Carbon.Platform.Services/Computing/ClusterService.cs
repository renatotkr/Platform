using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
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

        public Task<IReadOnlyList<Cluster>> ListAsync(IEnvironment environment)
        {
            Validate.NotNull(environment, nameof(environment));
            
            return db.Clusters.QueryAsync(
                And(Eq("environmentId", environment.Id), IsNull("deleted"))
            );
        }

        public async Task<Cluster> CreateAsync(CreateClusterRequest request)
        {
            Validate.NotNull(request, nameof(request));

            var cluster = new Cluster(
               id             : await db.Clusters.Sequence.NextAsync(),
               name           : request.Name,
               environmentId  : request.EnvironmentId,
               locationId     : request.LocationId,
               properties     : request.Properties,
               healthCheckId  : request.HealthCheckId,
               hostTemplateId : request.HostTemplate.Id
            );

            await db.Clusters.InsertAsync(cluster);

            return cluster;
        }

        public async Task<Cluster> GetAsync(long id)
        {
            return await db.Clusters.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.Cluster, id);
        }

        public async Task<Cluster> GetAsync(IEnvironment environment, ILocation location)
        {
            #region Preconditions

            if (environment == null)
                throw new ArgumentNullException(nameof(environment));

            if (location == null)
                throw new ArgumentNullException(nameof(location));

            #endregion

            var group = await db.Clusters.QueryFirstOrDefaultAsync(
                And(Eq("environmentId", environment.Id), Eq("locationId", location.Id))
            );
            
            if (group == null || group.Deleted != null)
            {
                throw new ResourceNotFoundException($"cluster(environment#{environment.Id}, location#{location.Id})");
            }
            
            return group;
        }

        public async Task<bool> DeleteAsync(ICluster cluster)
        {
            return await db.Clusters.PatchAsync(cluster.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;
        }
    }
}