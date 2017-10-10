using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Platform.Sequences;

using Dapper;

namespace Carbon.Platform.Computing
{
    internal class ProgramReleaseId
    {
        private static readonly string sql = SqlHelper.GetCurrentValueAndIncrement<ProgramInfo>("releaseCount");

        public static async Task<long> NextAsync(IDbContext context, long programId)
        {
            using (var connection = await context.GetConnectionAsync())
            {
                var currentReleaseCount = await connection.ExecuteScalarAsync<int>(sql, new { id = programId });

                return ScopedId.Create(programId, currentReleaseCount + 1);
            }
        }
    }
}