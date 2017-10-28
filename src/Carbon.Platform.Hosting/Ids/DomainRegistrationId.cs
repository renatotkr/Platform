using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Platform.Sequences;

using Dapper;

namespace Carbon.Platform.Hosting
{
    internal class DomainRegistrationId
    {
        private static readonly string sql = SqlHelper.GetCurrentValueAndIncrement<Domain>("registrationCount");

        public static async Task<long> NextAsync(IDbContext context, long domainId)
        {
            using (var connection = await context.GetConnectionAsync())
            {
                var currentValue = await connection.ExecuteScalarAsync<int>(sql, new { id = domainId });

                return ScopedId.Create(domainId, currentValue + 1);
            }
        }
    }
}