using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Sequences;

namespace Carbon.Rds.Services
{
    using static Expression;

    public class DatabaseGrantService : IDatabaseGrantService
    {
        private readonly RdsDb db;

        public DatabaseGrantService(RdsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<DatabaseGrant>> ListAsync(IDatabaseInfo database)
        {
            #region Preconditions

            if (database == null)
                throw new ArgumentNullException(nameof(database));

            #endregion

            var range = ScopedId.GetRange(database.Id);

            return db.DatabaseGrants.QueryAsync(
                And(Between("id", range.Start, range.End), IsNull("deleted"))
            );
        }

        public Task<IReadOnlyList<DatabaseGrant>> ListAsync(long userId) // IUser user
        {
            return db.DatabaseGrants.QueryAsync(
                And(Eq("userId", userId), IsNull("deleted"))
            );
        }

        public async Task<DatabaseGrant> CreateAsync(CreateDatabaseGrantRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var grantId = await DatabaseGrantId.NextAsync(db.Context, request.DatabaseId);

            var grant = new DatabaseGrant(
                id         : grantId,
                databaseId : request.DatabaseId,
                userId     : request.UserId,
                resource   : request.Resource,
                actions    : request.Actions
            );

            await db.DatabaseGrants.InsertAsync(grant);

            return grant;
        }

        public async Task DeleteAsync(IDatabaseGrant grant)
        {
            #region Preconditions

            if (grant == null)
                throw new ArgumentNullException(nameof(grant));

            #endregion

            await db.DatabaseGrants.PatchAsync(grant.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            });
        }
    }
}