using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;

namespace Carbon.Rds.Services
{
    using static Expression;

    public class DatabaseService : IDatabaseService
    {
        private readonly RdsDb db;

        private readonly DatabaseClusterService clusterService;

        public DatabaseService(RdsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));

            this.clusterService = new DatabaseClusterService(db);
        }

        public async Task<DatabaseInfo> GetAsync(long id)
        {
            Ensure.IsValidId(id);

            return await db.Databases.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.Database, id);
        }

        public Task<IReadOnlyList<DatabaseInfo>> ListAsync(long ownerId)
        {
            Ensure.IsValidId(ownerId, nameof(ownerId));

            return db.Databases.QueryAsync(
                And(Eq("ownerId", ownerId), IsNull("deleted"))
            );
        }

        public async Task<DatabaseInfo> RegisterAsync(RegisterDatabaseRequest request)
        {
            Ensure.NotNull(request, nameof(request));

            var databaseId = await db.Databases.Sequence.NextAsync();

            var database = new DatabaseInfo(databaseId, request.Name, request.OwnerId);
            
            await db.Databases.InsertAsync(database);

            if (request.Clusters != null)
            {
                foreach (var cluster in request.Clusters)
                {
                    cluster.DatabaseId = database.Id;

                    await clusterService.RegisterAsync(cluster);
                }
            }

            return database;
        }

        public async Task<bool> DeleteAsync(IDatabaseInfo database)
        {
            Ensure.NotNull(database, nameof(database));

            return await db.Databases.PatchAsync(database.Id, new[] {
                Change.Replace("deleted", Now)
            }, condition: IsNull("deleted")) > 0;
        }
    }
}