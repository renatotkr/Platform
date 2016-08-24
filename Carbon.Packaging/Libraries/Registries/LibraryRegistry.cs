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

        public async Task<IPackage> FindAsync(string name, Semver version)
        {
            var id = LookupId(name);

            return await table.QueryFirstOrDefaultAsync(Eq("id", id), Eq("version", version)).ConfigureAwait(false);
        }

        public async Task<IPackage> FindAsync(string name, SemverRange range)
        {
            var id = LookupId(name);

            var query = new Query(
                Order.Descending("id"), 
                Eq("id", id),
                Between("version", range.Start, range.End)) {
                Limit = 1
            };

            return await table.QueryFirstOrDefaultAsync(query).ConfigureAwait(false);
        }

        private long LookupId(string name)
        {
            return table.QueryFirstOrDefaultAsync(Eq("packageName", name)).Result.PackageId;
        }
    }
}