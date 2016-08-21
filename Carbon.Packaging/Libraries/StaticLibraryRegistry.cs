using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Carbon.Platform
{
    using Data;

    public class StaticLibraryRegistry : Collection<Library>, ILibraryRegistry
    {
        public StaticLibraryRegistry() { }

        internal StaticLibraryRegistry(Library[] list)
            : base(list)
        { }

        public Library Find(CryptographicHash hash)
            => this.Where(l =>  l.Hash == hash).First();

        public Library Find(string name, SemverRange range)
            => this.Where(l => l.Name == name).First();

        public Library Find(string name, Semver version)
            => this.Where(l => l.Name == name && l.Version == version).First();

        public string Serialize()
            => XNode.FromObject(this).ToString();

        public static StaticLibraryRegistry Deserialize(string text)
        {
            #region Preconditions

            if (text == null) throw new ArgumentNullException(nameof(text));

            if (text.Length == 0) throw new ArgumentException("Must not be empty", paramName: nameof(text));

            #endregion
            
            return new StaticLibraryRegistry(XArray.Parse(text).ToArrayOf<Library>());
        }
    }
}
