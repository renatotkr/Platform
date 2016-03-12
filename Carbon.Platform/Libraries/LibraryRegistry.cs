using System;
using System.Linq;
using System.Threading.Tasks;

using Carbon.Data;

using static Carbon.Data.Expression;

namespace Carbon.Platform
{
    public class LibraryRegistry2 : ILibraryRegistry
    {
        private readonly ITable<Library> table;

        public LibraryRegistry2(ITable<Library> table)
        {
            #region Preconditions

            if (table == null) throw new ArgumentNullException(nameof(table));

            #endregion

            this.table = table;
        }

        public async Task<Library> FindAsync(string name, Semver version)
        {
            var query = new Query(Eq("name", name), Eq("version", version)) {
                Limit = 1
            };

            return (await table.QueryAsync(query).ConfigureAwait(false)).FirstOrDefault();
        }

        public async Task<Library> FindAsync(string name, SemverRange range)
        {
            var query = new Query(Eq("name", name), Between("version", range.Start, range.End)) {
                Limit = 1,
                Order = QueryOrder.Descending
            };

            return (await table.QueryAsync(query).ConfigureAwait(false)).FirstOrDefault();
        }

        public Library Find(string name, Semver version)
            => FindAsync(name, version).Result;

        public Library Find(string name, SemverRange range)
            => FindAsync(name, range).Result;
    }
}