using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Platform.Sequences;

using Dapper;

namespace Carbon.Kms
{
    public class SecretId
    {
        static readonly string sql = SqlHelper.GetCurrentValueAndIncrement<VaultInfo>("secretCount");

        public static async Task<long> NextAsync(IDbContext context, long vaultId)
        {
            using (var connection = await context.GetConnectionAsync())
            {
                var currentCount = await connection.ExecuteScalarAsync<int>(sql, new { id = vaultId }).ConfigureAwait(false);

                return ScopedId.Create(vaultId, currentCount + 1);
            }
        }
    }
}