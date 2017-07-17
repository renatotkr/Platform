using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Storage;

namespace Carbon.Rds.Services
{
    using static Expression;

    public class DatabaseUserService : IDatabaseUserService
    {
        private readonly RdsDb db;

        public DatabaseUserService(RdsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<DatabaseUser>> ListAsync(IDatabaseInfo database)
        {
            #region Preconditions

            if (database == null)
                throw new ArgumentNullException(nameof(database));

            #endregion

            return db.DatabaseUsers.QueryAsync(
                And(Eq("databaseId", database.Id), IsNull("deleted"))
            );
        }

        public Task<bool> ExistsAsync(long databaseId, long userId)
        {
            return db.DatabaseUsers.ExistsAsync(
                And(Eq("databaseId", databaseId), Eq("userId", userId))
            );
        }

        public async Task<DatabaseUser> CreateAsync(CreateDatabaseUserRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var dbUser = new DatabaseUser(request.DatabaseId, request.UserId, request.Name);
            
            await db.DatabaseUsers.InsertAsync(dbUser);

            return dbUser;
        }

        public async Task DeleteAsync(DatabaseUser user)
        {
            await db.DatabaseUsers.PatchAsync((user.DatabaseId, user.UserId), new[] {
                Change.Replace("deleted", Func("NOW"))
            });
        }
    }
}