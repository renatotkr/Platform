using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Platform.Sequences;

using Dapper;

namespace Carbon.Rds
{
    internal static class DatabaseGrantId
    {
        private static readonly string sql = SqlHelper.GetCurrentValueAndIncrement<DatabaseInfo>("grantCount");

        public static async Task<long> NextAsync(IDbContext context, long databaseId)
        {
            using (var connection = await context.GetConnectionAsync())
            {
                var currentCount = await connection.ExecuteScalarAsync<int>(sql, new { id = databaseId });

                return ScopedId.Create(databaseId, currentCount + 1);
            }
        }
    }
}
