using System;
using System.Collections.Generic;
using System.Linq;

namespace Carbon.Packaging
{
    using Storage;

    public class Manifest : Dictionary<string, IManifestEntry>
    {
        public Manifest(IEnumerable<IBlob> blobs)
        {
            foreach (var blob in blobs)
            {
                Add(blob.Name, ManifestEntry.FromBlob(blob));
            }
        }

        public Manifest(IEnumerable<IManifestEntry> items)
        {
            foreach (var item in items)
            {
                Add(item.Path, item);
            }
        }

        public bool TryFind(string name, out IManifestEntry item)
        {
            #region Preconditions

            if (name == null) throw new ArgumentNullException(nameof(name));

            #endregion

            if (name[0] == '/')
            {
                name = name.Trim(Seperators.ForwardSlash);
            }

            return TryGetValue(name, out item);
        }

        public bool Contains(string name) => ContainsKey(name);

        public static Manifest FromPackage(Package package)
        {
            return new Manifest(package.Enumerate().Select(blob => (IManifestEntry)ManifestEntry.FromBlob(blob)));
        }
    }
}