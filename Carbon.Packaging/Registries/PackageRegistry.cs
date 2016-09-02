using System;
using System.Threading.Tasks;

using static Carbon.Data.Expression;

namespace Carbon.Packaging
{
    using Data;

    public class PackageRegistry : IPackageRegistry
    {
        private readonly IPackageDb db;

        public PackageRegistry(IPackageDb db)
        {
            #region Preconditions

            if (db == null) throw new ArgumentNullException(nameof(db));

            #endregion

            this.db = db;
        }

        public long LookupId(string name) 
            => db.Packages.QueryFirstOrDefaultAsync(Eq("name", name)).Result.Id;

        public async Task<PackageInfo> FindAsync(long id, bool includeReleases = true)
        {
            var package = await db.Packages.GetAsync(new RecordKey("id", id)).ConfigureAwait(false);

            if (includeReleases)
            {
                package.Releases = await db.PackageReleases.QueryAsync(
                    new Query(Order.Descending("version"),
                    Eq("id", id))
                ).ConfigureAwait(false); 
            }

            return package;
        }

        public async Task<IPackage> FindAsync(long id, Semver version)
        {
            if (version.Level == SemverLevel.Patch && version.Flags == SemverFlags.None)
            {
                return await db.PackageReleases.QueryFirstOrDefaultAsync(
                    Eq("id", id),
                    Eq("version", version)
                ).ConfigureAwait(false);
            }
            else
            {
                var range = version.GetRange();

                var query = new Query(
                   Order.Descending("id"),
                   Eq("id", id),
                   Between("version", range.Start, range.End)
                );

                return await db.PackageReleases.QueryFirstOrDefaultAsync(query).ConfigureAwait(false);
            }
        }

        public Task<PackageInfo> FindAsync(Hash hash)
            => db.PackageReleases.QueryFirstOrDefaultAsync(Eq("hash", hash));

        public async Task CreateAsync(PackageInfo release)
        {
            #region Preconditions

            if (release == null)
                throw new ArgumentNullException(nameof(release));

            if (release.Name == null)
                throw new ArgumentNullException("release.Name");

            #endregion

            if (release.Version.Level != SemverLevel.Minor)
                throw new Exception("All version levels must be assigned");

            if (release.Version.Flags != SemverFlags.None)
                throw new Exception("A release must have a fixed version number");


            // if it's the latest version, update the main record

            await db.PackageReleases.InsertAsync(release).ConfigureAwait(false);
        }
    }

    public interface IPackageDb
    {
        ITable<PackageInfo> Packages        { get; }
        ITable<PackageInfo> PackageReleases { get; }
    }
}