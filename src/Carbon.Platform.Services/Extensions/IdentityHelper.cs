using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;

using Carbon.Platform.Sequences;

using Dapper;

namespace Carbon.Platform
{
    using static Expression;

    internal static class IdentityHelper
    {
        public static long GetNextId<T>(this IDbContext context)
        {
            var dataset = DatasetInfo.Get<T>();

            using (var connection = context.GetConnection())
            {
                var id = connection.ExecuteScalar<long>($"SELECT `id` FROM `{dataset.Name}` ORDER BY `id` DESC LIMIT 1");

                return id + 1;
            }
        }

        public static async Task<long> GetNextScopedIdAsync<T>(this Dataset<T, long> dataset, long scopeId)
        {
            var range = ScopedId.GetRange(scopeId);

            var count = await dataset.CountAsync(Between("id", range.Start, range.End)).ConfigureAwait(false);

            return ScopedId.Create(scopeId, count);
        }
    }
}
