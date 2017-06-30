using System;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Platform;
using Carbon.Platform.Environments;

using Dapper;

namespace Carbon.CI
{
    internal static class DeploymentId
    {
        private static readonly string sql = SqlHelper.GetCurrentValueAndIncrement<EnvironmentInfo>("commandCount");

        public static async Task<long> NextAsync(IDbContext context, IEnvironment environment)
        {
            using (var connection = await context.GetConnectionAsync())
            {
                return await connection.ExecuteScalarAsync<int>(sql, environment).ConfigureAwait(false) + 1;
            }
        }
    }
}