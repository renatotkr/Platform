using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform;
using Carbon.Platform.Sequences;

namespace Carbon.Rds.Services
{
    using static Expression;

    public class DatabaseClusterService : IDatabaseClusterService
    {
        private readonly RdsDb db;

        private readonly DatabaseInstanceService instanceService;

        public DatabaseClusterService(RdsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));

            this.instanceService = new DatabaseInstanceService(db);
        }

        public Task<IReadOnlyList<DatabaseCluster>> ListAsync(IDatabaseInfo database)
        {
            Validate.NotNull(database, nameof(database));

            var range = ScopedId.GetRange(database.Id);

            return db.DatabaseClusters.QueryAsync(
                And(Between("id", range.Start, range.End), IsNull("deleted"))
            );
        }

        public async Task<DatabaseCluster> RegisterAsync(RegisterDatabaseClusterRequest request)
        {
            Validate.NotNull(request, nameof(request));
            
            #region Preconditions
            
            if (request.Name == null)
                throw new ArgumentNullException(nameof(request.Name));

            if (request.DatabaseId <= 0)
                throw new ArgumentException("Must be > 0", nameof(request.DatabaseId));

            if (request.Resource.LocationId <= 0)
                throw new ArgumentException("Must be > 0", "locationId");

            #endregion

            var location = Locations.Get(request.Resource.LocationId);

            var databaseId = request.DatabaseId;

            var clusterId = await DatabaseClusterId.NextAsync(db.Context, request.DatabaseId);

            var cluster = new DatabaseCluster(
                id       : clusterId,
                name     : request.Name,
                resource : request.Resource
            );

            await db.DatabaseClusters.InsertAsync(cluster);

            // Create the cluster's instances
            if (request.Instances != null)
            {
                foreach (var instance in request.Instances)
                {
                    instance.DatabaseId = databaseId;
                    instance.ClusterId  = clusterId;

                    await instanceService.RegisterAsync(instance);
                }
            }

            // Create the cluster's endpoints
            if (request.Endpoints != null)
            {
                foreach (var e in request.Endpoints)
                {
                    var endpointId = await DatabaseEndpointId.NextAsync(db.Context, databaseId);

                    var endpoint = new DatabaseEndpoint(
                        id       : endpointId, 
                        host     : e.Host,
                        location : location, 
                        port     : (ushort)e.Port,
                        flags    : e.Flags
                    );

                    await db.DatabaseEndpoints.InsertAsync(endpoint);
                }
            }

            return cluster;
        }
    }
}