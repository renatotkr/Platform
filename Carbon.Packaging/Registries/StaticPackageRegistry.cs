using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Carbon.Packaging
{
    using Data;

    public class StaticPackageRegistry : Collection<PackageRelease>, IPackageRegistry
    {
        public StaticPackageRegistry() { }

        internal StaticPackageRegistry(PackageRelease[] list)
            : base(list)
        { }

        public long LookupId(string name) 
            => this.First(p => p.Name == name).Id;

        public Task<IPackage> FindAsync(long id, Semver version)
            => Task.FromResult<IPackage>(this.FirstOrDefault(l => l.Id == id && l.Version == version));

        public string Serialize()
            => XNode.FromObject(this).ToString();

        public static StaticPackageRegistry Deserialize(string text)
        {
            #region Preconditions

            if (text == null) throw new ArgumentNullException(nameof(text));

            if (text.Length == 0) throw new ArgumentException("Must not be empty", paramName: nameof(text));

            #endregion
         
            // TODO: Use protobuff....
               
            return new StaticPackageRegistry(XArray.Parse(text).ToArrayOf<PackageRelease>());
        }

        public Task CreateAsync(PackageRelease package)
        {
            throw new NotImplementedException();
        }
    }
}