using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Platform;

using Dapper;

namespace Carbon.CI
{
    public static class DeploymentId
    {
        private static readonly string sql = SqlHelper.GetCurrentValueAndIncrement<EnvironmentInfo>("deploymentCount");

        public static async Task<long> NextAsync(IDbContext context, IEnvironment env)
        {
            using (var connection = context.GetConnection())
            {
                return await connection.ExecuteScalarAsync<int>(sql, env).ConfigureAwait(false) + 1;
            }
        }
    }
}