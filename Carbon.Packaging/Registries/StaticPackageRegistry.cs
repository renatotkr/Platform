using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Carbon.Packaging
{
    using Json;

    public class StaticPackageRegistry : Collection<PackageInfo>, IPackageRegistry
    {
        public StaticPackageRegistry() { }

        internal StaticPackageRegistry(PackageInfo[] list)
            : base(list)
        { }

        public long Lookup(string name) 
            => this.First(p => p.Name == name).Id;

        public Task<IPackage> GetAsync(long id, Semver version)
            => Task.FromResult<IPackage>(this.FirstOrDefault(l => l.Id == id && l.Version == version));

        public string Serialize()
            => JsonObject.FromObject(this).ToString();

        public static StaticPackageRegistry Deserialize(string text)
        {
            #region Preconditions

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (text.Length == 0)
                throw new ArgumentException("Must not be empty", paramName: nameof(text));

            #endregion
         
            // TODO: Use protobuff....
               
            return new StaticPackageRegistry(JsonArray.Parse(text).ToArrayOf<PackageInfo>());
        }

        public Task CreateAsync(PackageInfo package)
        {
            throw new NotImplementedException();
        }
    }
}