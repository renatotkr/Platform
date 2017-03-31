using System;
using System.Collections.Generic;
using System.Linq;

namespace Carbon.Packaging
{
    using Storage;

    // TODO: Diffs between two manifests...

    public class PackageManifest : Dictionary<string, ManifestEntry>
    {
        public PackageManifest(IEnumerable<IBlob> blobs)
        {
            foreach (var blob in blobs)
            {
                Add(blob.Name, ManifestEntry.FromBlob(blob));
            }
        }

        public PackageManifest(IEnumerable<ManifestEntry> items)
        {
            foreach (var item in items)
            {
                Add(item.Name, item);
            }
        }

        public bool TryFind(string name, out ManifestEntry item)
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

        public static PackageManifest FromPackage(Package package)
        {
            return new PackageManifest(package.Enumerate().Select(blob => ManifestEntry.FromBlob(blob)));
        }
    }
}