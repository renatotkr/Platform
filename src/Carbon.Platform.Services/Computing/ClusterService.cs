using System;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

using Dapper;

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
        
        public async Task<Cluster> CreateAsync(CreateClusterRequest request)
        {
            #region Preconditions

            if (LocationId.Create(request.Location.Id).ZoneNumber > 0)
                throw new ArgumentException("Must be a region. Was a zone.", nameof(request.Location));

            // TODO: Add support for zone groups

            #endregion
            
            // e.g. carbon/us-east-1

            var group = new Cluster(
               id            : db.Clusters.Sequence.Next(),
               name          : request.Environment.Name + "/" + request.Location.Name,
               environmentId : request.Environment.Id,
               details       : request.Details,
               resource      : ManagedResource.Cluster(request.Location, Guid.NewGuid().ToString())
            );

            await db.Clusters.InsertAsync(group).ConfigureAwait(false);

            return group;
        }

        public async Task<Cluster> GetAsync(long id)
        {
            return await db.Clusters.FindAsync(id) ?? throw ResourceError.NotFound(ResourceTypes.Cluster, id);
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

        public async Task<ClusterResource> AddResourceAsync(Cluster cluster, IResource resource)
        {
            var location = Locations.Get(cluster.LocationId);

            var id = await ClusterResourceId.NextAsync(db.Context, cluster.Id).ConfigureAwait(false);

            var record = new ClusterResource(
                id            : id,
                cluster       : cluster,
                resource      : resource,
                environmentId : cluster.EnvironmentId,
                location      : location
            );

            await db.ClusterResources.InsertAsync(record).ConfigureAwait(false);

            return record;
        }
    }

    internal static class ClusterResourceId
    {
        public static async Task<long> NextAsync(
            IDbContext context,
            long clusterId)
        {
            using (var connection = context.GetConnection())
            {
                var currentResourceCount = await connection.ExecuteScalarAsync<int>(
                  @"SELECT `resourceCount` FROM `Clusters` WHERE id = @id FOR UPDATE;
                      UPDATE `Clusters`
                      SET `resourceCount` = `resourceCount` + 1
                      WHERE id = @id", new { id = clusterId }).ConfigureAwait(false);

                return ScopedId.Create(clusterId, currentResourceCount + 1);
            }
        }
    }
}