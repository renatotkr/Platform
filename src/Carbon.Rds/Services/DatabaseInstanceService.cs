using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Sequences;

namespace Carbon.Rds.Services
{
    using static Expression;

    public class DatabaseInstanceService : IDatabaseInstanceService
    {
        private readonly RdsDb db;

        public DatabaseInstanceService(RdsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<DatabaseInstance>> ListAsync(IDatabaseInfo database)
        {
            Ensure.NotNull(database, nameof(database));

            var range = ScopedId.GetRange(database.Id);

            return db.DatabaseInstances.QueryAsync(
                And(Between("id", range.Start, range.End), IsNull("deleted"))
            );
        }

        public async Task<DatabaseInstance> RegisterAsync(RegisterDatabaseInstanceRequest request)
        {
            Ensure.NotNull(request, nameof(request));

            var instanceId = await DatabaseInstanceId.NextAsync(db.Context, request.DatabaseId);

            var instance = new DatabaseInstance(
                id        : instanceId,
                clusterId : request.ClusterId,
                flags     : request.Flags,
                priority  : request.Priority,
                resource  : request.Resource
            );

            await db.DatabaseInstances.InsertAsync(instance);

            return instance;
        }

        public async Task DeleteAsync(DatabaseInstance instance)
        {
            Ensure.NotNull(instance, nameof(instance));
            
            await db.DatabaseInstances.PatchAsync(instance.Id, new[] {
                Change.Replace("terminated", Func("NOW"))
            });
        }
    }
}