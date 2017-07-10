using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;

using Carbon.Platform.Sequences;

namespace Carbon.Platform
{
    internal static class IdentityHelper
    {
        public static async Task<long> GetNextScopedIdAsync<T>(this Dataset<T, long> dataset, long scopeId)
        {
            var range = ScopedId.GetRange(scopeId);

            // should be max id in range...
            var count = await dataset.CountAsync(
                Expression.Between("id", range.Start, range.End)
            ).ConfigureAwait(false);

            return ScopedId.Create(scopeId, count);
        }
    }
}
