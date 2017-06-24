using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Platform.Sequences;

using Dapper;

namespace Carbon.CI
{
    internal static class BuildId
    {
        private static readonly string sql = SqlHelper.GetCurrentValueAndIncrement<ProjectInfo>("buildCount");

        public static async Task<long> NextAsync(
            IDbContext context,
            long projectId)
        {
            using (var connection = context.GetConnection())
            {
                var currentCommitCount = await connection.ExecuteScalarAsync<int>(sql, 
                    new { id = projectId }
                ).ConfigureAwait(false);

                return ScopedId.Create(projectId, currentCommitCount + 1);
            }
        }
    }
}