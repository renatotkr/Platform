using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Platform.Sequences;

using Dapper;

namespace Carbon.CI
{
    internal static class CommitId
    {
        private static readonly string sql = SqlHelper.GetCurrentValueAndIncrement<RepositoryInfo>("commitCount");

        public static async Task<long> NextAsync(IDbContext context, long repositoryId)
        {
            using (var connection = await context.GetConnectionAsync())
            {
                var currentCount = await connection.ExecuteScalarAsync<int>(sql, 
                    new { id = repositoryId });

                return ScopedId.Create(repositoryId, currentCount + 1);
            }
        }
    }
}