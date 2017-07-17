using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Data.Expressions;

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

        public Task<IReadOnlyList<DatabaseInfo>> ListAsync(long ownerId)
        {
            return db.Databases.QueryAsync(
                And(Eq("ownerId", ownerId), IsNull("deleted"))
            );
        }

        public async Task<DatabaseInfo> RegisterAsync(RegisterDatabaseRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

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
    }
}