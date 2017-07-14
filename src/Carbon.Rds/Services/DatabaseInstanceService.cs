using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Sequences;

namespace Carbon.Rds.Services
{
    public class DatabaseInstanceService : IDatabaseInstanceService
    {
        private readonly RdsDb db;

        public DatabaseInstanceService(RdsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<DatabaseInstance>> ListAsync(IDatabaseInfo database)
        {
            #region Preconditions

            if (database == null)
                throw new ArgumentNullException(nameof(database));

            #endregion

            var range = ScopedId.GetRange(database.Id);

            return db.DatabaseInstances.QueryAsync(
                Expression.And(Expression.Between("id", range.Start, range.End), Expression.IsNotNull("deleted"))
            );
        }

        public async Task<DatabaseInstance> RegisterAsync(RegisterDatabaseInstanceRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

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
            #region Preconditions

            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            #endregion

            await db.DatabaseInstances.PatchAsync(instance.Id, new[] {
                Change.Replace("terminated", Expression.Func("NOW"))
            });
        }
    }
}