using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Carbon.Packaging
{
    using Data;

    public class StaticPackageManager : Collection<LibraryRelease>, IPackageRegistry
    {
        public StaticPackageManager() { }

        internal StaticPackageManager(LibraryRelease[] list)
            : base(list)
        { }

        public LibraryRelease Find(CryptographicHash hash)
            => this.First(l => l.Hash == hash);

        public Task<IPackage> FindAsync(string name, SemverRange range)
            => Task.FromResult<IPackage>(this.First(l => l.LibraryName == name));

        public Task<IPackage> FindAsync(string name, Semver version)
            => Task.FromResult<IPackage>(this.FirstOrDefault(l => l.LibraryName == name && l.Version == version));

        public string Serialize()
            => XNode.FromObject(this).ToString();

        public static StaticPackageManager Deserialize(string text)
        {
            #region Preconditions

            if (text == null) throw new ArgumentNullException(nameof(text));

            if (text.Length == 0) throw new ArgumentException("Must not be empty", paramName: nameof(text));

            #endregion
         
            // TODO: Use protobuff....
               
            return new StaticPackageManager(XArray.Parse(text).ToArrayOf<LibraryRelease>());
        }
    }
}