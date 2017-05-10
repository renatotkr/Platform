using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;

using Carbon.Platform.Sequences;

namespace Carbon.Platform
{
    using static Expression;

    internal static class IdentityHelper
    {
        public static async Task<long> GetNextScopedIdAsync<T>(this Dataset<T, long> dataset, long scopeId)
        {
            var range = ScopedId.GetRange(scopeId);

            var count = await dataset.CountAsync(Between("id", range.Start, range.End)).ConfigureAwait(false);

            return ScopedId.Create(scopeId, count);
        }
    }
}
