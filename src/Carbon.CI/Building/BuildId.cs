using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Platform.Sequences;

using Dapper;

namespace Carbon.CI
{
    internal static class BuildId
    {
        public static async Task<long> NextAsync(
            IDbContext context,
            long projectId)
        {
            using (var connection = context.GetConnection())
            {
                var currentCommitCount = await connection.ExecuteScalarAsync<int>(
                  @"SELECT `buildCount` FROM `BuildProjects` WHERE id = @id FOR UPDATE;
                      UPDATE `BuildProjects`
                      SET `buildCount` = `buildCount` + 1
                      WHERE id = @id", new { id = projectId }).ConfigureAwait(false);

                return ScopedId.Create(projectId, currentCommitCount + 1);
            }
        }
    }
}


// arn:aws:codebuild:region-ID:account-ID:build