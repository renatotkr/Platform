using System;
using System.Collections.ObjectModel;

namespace Carbon.Libraries
{
    using Json;
    using Versioning;

    public class StaticLibraryRegistry : Collection<ILibrary>, ILibraryRegistry
    {
        public StaticLibraryRegistry() { }

        internal StaticLibraryRegistry(Library[] list)
            : base(list)
        { }

        public ILibrary Find(string name, SemanticVersionRange range)
        {
            foreach (var library in this)
            {
                if (library.Name == name) return library;
            }

            throw new Exception($"A library named '{name}' was not found");
        }

        public ILibrary Find(string name, SemanticVersion version)
        {
            foreach (var library in this)
            {
                if (library.Name == name && library.Version == version)
                {
                    return library;
                }
            }

            return null;
        }

        public string Serialize()
            => JsonNode.FromObject(this).ToString(pretty: true);

        public static ILibraryRegistry Deserialize(string text)
        {
            #region Preconditions

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (text.Length == 0)
                throw new ArgumentException("Must not be empty", nameof(text));

            #endregion

            var a = JsonArray.Parse(text).ToArrayOf<Library>();

            return new StaticLibraryRegistry(a);
        }
    }
}

/*

[
  { 
    name       : "basejs", 
    version    : "1.0.0",
    main       : { name: "hi.json", sha256: "base64encoded" }
  },
  ...
]

*/
