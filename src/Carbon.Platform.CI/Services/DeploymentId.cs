using System.Threading.Tasks;

using Carbon.Data;

using Dapper;

namespace Carbon.Platform.CI
{
    public static class DeploymentId
    {
        public static async Task<long> GetNextAsync(IDbContext context, IEnvironment env)
        {
            using (var connection = context.GetConnection())
            using (var ts = connection.BeginTransaction())
            {
                var result = await connection.ExecuteScalarAsync<int>(
                    @"SELECT `deploymentCount` FROM `Environments` WHERE id = @id FOR UPDATE;
                      UPDATE `Environments`
                      SET `deploymentCount` = `deploymentCount` + 1
                      WHERE id = @id", env, ts).ConfigureAwait(false);

                ts.Commit();

                return result + 1;
            }
        }
    }
}
