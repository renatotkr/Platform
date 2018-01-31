using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Sequences;

namespace Carbon.Rds.Services
{
    using static Expression;

    public class DatabaseSchemaService : IDatabaseSchemaService
    {
        private readonly RdsDb db;

        public DatabaseSchemaService(RdsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<DatabaseBackup>> ListAsync(IDatabaseInfo database)
        {
            Ensure.NotNull(database, nameof(database));

            var range = ScopedId.GetRange(database.Id);

            return db.DatabaseBackups.QueryAsync(Between("id", range.Start, range.End), Order.Descending("id"));
        }

        public async Task<DatabaseSchema> CreateAsync(CreateDatabaseSchemaRequest request)
        {
            var schemaId = await DatabaseSchemaId.NextAsync(db.Context, request.DatabaseId);

            var schema = new DatabaseSchema(schemaId, request.SchemaName);

            await db.DatabaseSchemas.InsertAsync(schema);

            return schema;
        }

        public async Task DeleteAsync(IDatabaseSchema schema)
        {
            Ensure.NotNull(schema, nameof(schema));

            await db.DatabaseSchemas.PatchAsync(schema.Id, new[] {
                Change.Replace("deleted", Now)
            }, condition: IsNull("deleted"));
        }
    }
}