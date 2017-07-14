using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform;
using Carbon.Platform.Sequences;

namespace Carbon.Rds.Services
{
    using static Expression;

    public class DatabaseEndpointService : IDatabaseEndpointService
    {
        private readonly RdsDb db;

        public DatabaseEndpointService(RdsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<DatabaseEndpoint>> ListAsync(IDatabaseInfo database)
        {
            #region Preconditions

            if (database == null)
                throw new ArgumentNullException(nameof(database));

            #endregion

            var range = ScopedId.GetRange(database.Id);

            return db.DatabaseEndpoints.QueryAsync(
                And(Between("id", range.Start, range.End), IsNotNull("deleted"))
            );
        }
    }
}