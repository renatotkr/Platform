using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;

using Carbon.Platform.Sequences;

namespace Carbon.Rds.Services
{
    public class DatabaseMigrationService : IDatabaseMigrationService
    {
        private readonly RdsDb db;

        public DatabaseMigrationService(RdsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<DatabaseMigration>> ListAsync(IDatabaseInfo database)
        {
            #region Preconditions

            if (database == null)
                throw new ArgumentNullException(nameof(database));

            #endregion

            var range = ScopedId.GetRange(database.Id);

            return db.DatabaseMigrations.QueryAsync(
                Expression.Between("id", range.Start, range.End),
                Order.Descending("id")
            );
        }

        public async Task<DatabaseMigration> CreateAsync(CreateDatabaseMigrationRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var migrationId = await DatabaseMigrationId.NextAsync(db.Context, request.DatabaseId);

            var migration = new DatabaseMigration(
                id          : migrationId,
                schemaName  : request.SchemaName,
                commands    : request.Commands,
                description : request.Description
            );

            await db.DatabaseMigrations.InsertAsync(migration);

            return migration;
        }
    }
}