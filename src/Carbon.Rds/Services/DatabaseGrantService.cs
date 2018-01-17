using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Data.Sql;
using Carbon.Platform.Sequences;
using Carbon.Security;

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
            Ensure.NotNull(database, nameof(database));

            var range = ScopedId.GetRange(database.Id);

            return db.DatabaseGrants.QueryAsync(
                And(Between("id", range.Start, range.End), IsNull("deleted"))
            );
        }

        public Task<IReadOnlyList<DatabaseGrant>> ListAsync(IUser user)
        {
            Ensure.NotNull(user, nameof(user));

            return db.DatabaseGrants.QueryAsync(
                And(Eq("userId", user.Id), IsNull("deleted"))
            );
        }

        public Task<IReadOnlyList<DatabaseGrant>> ListAsync(IDatabaseInfo database, IUser user)
        {
            Ensure.NotNull(database, nameof(database));
            Ensure.NotNull(user, nameof(user));

            var range = ScopedId.GetRange(database.Id);

            return db.DatabaseGrants.QueryAsync(
                Conjunction(
                    Between("id", range.Start, range.End),
                    Eq("userId", user.Id),
                    IsNull("deleted")
                )
            );
        }

        public Task<IReadOnlyList<DatabaseGrant>> ListAsync(IDatabaseInfo database, IUser user, DbObject resource)
        {
            Ensure.NotNull(database, nameof(database));
            Ensure.NotNull(user, nameof(user));

            var range = ScopedId.GetRange(database.Id);

            return db.DatabaseGrants.QueryAsync(
                Conjunction(
                    Between("id", range.Start, range.End),
                    Eq("userId", user.Id),
                    Eq("schemaName", resource.SchemaName),
                    Eq("objectType", resource.Type),
                    Eq("objectName", resource.ObjectName),
                    IsNull("deleted")
                )
            );
        }

        public async Task<DatabaseGrant> CreateAsync(CreateDatabaseGrantRequest request)
        {
            Ensure.NotNull(request, nameof(request));

            var grantId = await DatabaseGrantId.NextAsync(db.Context, request.DatabaseId);

            var grant = new DatabaseGrant(
                id         : grantId,
                databaseId : request.DatabaseId,
                userId     : request.UserId,
                resource   : request.Resource,
                privileges : request.Privileges
            );

            await db.DatabaseGrants.InsertAsync(grant);

            return grant;
        }

        public async Task<bool> DeleteAsync(IDatabaseGrant grant)
        {
            Ensure.NotNull(grant, nameof(grant));

            return await db.DatabaseGrants.PatchAsync(grant.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;
        }
    }
}