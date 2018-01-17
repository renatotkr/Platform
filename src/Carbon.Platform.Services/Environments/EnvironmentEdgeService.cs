using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;

namespace Carbon.Platform.Environments
{
    public class EnvironmentEdgeService : IEnvironmentEdgeService
    {
        private readonly PlatformDb db;

        public EnvironmentEdgeService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<EnvironmentEdge>> ListAsync(IEnvironment environment)
        {
            Ensure.NotNull(environment, nameof(environment));

            return db.EnvironmentEdges.QueryAsync(
                Expression.And(Expression.Eq("environmentId", environment.Id), Expression.IsNull("deleted"))
            );
        }

        public Task<EnvironmentEdge> GetAsync(long environmentId, int locationId)
        {
            return db.EnvironmentEdges.FindAsync((environmentId, locationId));
        }

        // TODO: Batch insert

        public async Task<EnvironmentEdge> CreateAsync(CreateEnvironmentEdgeRequest request)
        {
            Ensure.NotNull(request, nameof(request));

            var edge = new EnvironmentEdge(
                environmentId  : request.EnvironmentId,
                locationId     : request.LocationId,
                distributionId : request.DistributionId
            );
            
            await db.EnvironmentEdges.InsertAsync(edge);

            return edge;
        }

        public async Task ActivateAsync(EnvironmentEdge record)
        {
            Ensure.NotNull(record, nameof(record));

            await db.EnvironmentEdges.PatchAsync((record.EnvironmentId, record.LocationId), new[] {
                Change.Replace("activated", Expression.Func("NOW"))
            });
        }

        public async Task DeactivateAsync(EnvironmentEdge record)
        {
            Ensure.NotNull(record, nameof(record));

            await db.EnvironmentEdges.PatchAsync((record.EnvironmentId, record.LocationId), new[] {
                Change.Remove("activated")
            });
        }

        public async Task DeleteAsync(EnvironmentEdge record)
        {
            Ensure.NotNull(record, nameof(record));

            await db.EnvironmentEdges.PatchAsync((record.EnvironmentId, record.LocationId), new[] {
                Change.Replace("deleted", Expression.Func("NOW"))
            });
        }
    }
}