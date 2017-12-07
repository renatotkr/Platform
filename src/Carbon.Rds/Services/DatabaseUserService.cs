using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Data;
using Carbon.Data.Expressions;

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
            Validate.NotNull(database, nameof(database));

            return db.DatabaseUsers.QueryAsync(
                And(Eq("databaseId", database.Id), IsNull("deleted"))
            );
        }

        public Task<DatabaseUser> GetAsync(long databaseId, long userId)
        {
            return db.DatabaseUsers.FindAsync((databaseId, userId)); // or throw
        }

        public async Task<DatabaseUser> FindAsync(long databaseId, string name)
        {
            return await db.DatabaseUsers.QueryFirstOrDefaultAsync(
                Conjunction(
                    Eq("databaseId", databaseId),
                    Eq("name", name),
                    IsNull("deleted")
                )
            );
        }

        public Task<bool> ExistsAsync(long databaseId, long userId)
        {
            return db.DatabaseUsers.ExistsAsync(
                And(Eq("databaseId", databaseId), Eq("userId", userId))
            );
        }
        
        public async Task RestoreAsync(long databaseId, long userId)
        {
            await db.DatabaseUsers.PatchAsync((databaseId, userId), new[] {
                Change.Remove("deleted")
            });
        }

        public async Task<DatabaseUser> CreateAsync(CreateDatabaseUserRequest request)
        {
            Validate.NotNull(request, nameof(request));

            var dbUser = new DatabaseUser(request.DatabaseId, request.UserId, request.Name);
            
            await db.DatabaseUsers.InsertAsync(dbUser);

            return dbUser;
        }

        public async Task DeleteAsync(DatabaseUser user)
        {
            Validate.NotNull(user, nameof(user));

            await db.DatabaseUsers.PatchAsync((user.DatabaseId, user.UserId), new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted"));
        }
    }
}