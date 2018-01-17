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
            Ensure.NotNull(database, nameof(database));

            var range = ScopedId.GetRange(database.Id);

            return db.DatabaseMigrations.QueryAsync(
                Expression.Between("id", range.Start, range.End),
                Order.Descending("id")
            );
        }

        public async Task<DatabaseMigration> CreateAsync(CreateDatabaseMigrationRequest request)
        {
            Ensure.NotNull(request, nameof(request));

            var id = await DatabaseMigrationId.NextAsync(db.Context, request.DatabaseId);

            var migration = new DatabaseMigration(
                id          : id,
                schemaName  : request.SchemaName,
                commands    : request.Commands,
                description : request.Description
            );

            await db.DatabaseMigrations.InsertAsync(migration);

            return migration;
        }
    }
}