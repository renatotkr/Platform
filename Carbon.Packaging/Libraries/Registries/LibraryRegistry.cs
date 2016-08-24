using System;
using System.Threading.Tasks;

using static Carbon.Data.Expression;

namespace Carbon.Packaging
{
    using Data;

    public class LibraryRegistry2 : IPackageRegistry
    {
        private readonly ITable<PackageRelease> table;

        public LibraryRegistry2(ITable<PackageRelease> table)
        {
            #region Preconditions

            if (table == null) throw new ArgumentNullException(nameof(table));

            #endregion

            this.table = table;
        }

        public async Task<IPackage> FindAsync(long id, Semver version)
        {
            return await table.QueryFirstOrDefaultAsync(Eq("id", id), Eq("version", version)).ConfigureAwait(false);
        }

        public async Task<IPackage> FindAsync(long id, SemverRange range)
        {
            var query = new Query(
                Order.Descending("id"), 
                Eq("id", id),
                Between("version", range.Start, range.End)) {
                Limit = 1
            };

            return await table.QueryFirstOrDefaultAsync(query).ConfigureAwait(false);
        }

        public long LookupId(string name)
        {
            return table.QueryFirstOrDefaultAsync(Eq("packageName", name)).Result.Id;
        }
    }
}