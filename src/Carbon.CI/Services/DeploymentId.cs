using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Platform;

using Dapper;

namespace Carbon.CI
{
    public static class DeploymentId
    {
        public static async Task<long> NextAsync(IDbContext context, IEnvironment env)
        {
            using (var connection = context.GetConnection())
            {
                var result = await connection.ExecuteScalarAsync<int>(
                    @"SELECT `deploymentCount` FROM `Environments` WHERE id = @id FOR UPDATE;
                      UPDATE `Environments`
                      SET `deploymentCount` = `deploymentCount` + 1
                      WHERE id = @id", env).ConfigureAwait(false);

                return result + 1;
            }
        }
    }
}