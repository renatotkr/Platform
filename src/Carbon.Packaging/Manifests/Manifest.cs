using System;
using System.Collections.Generic;
using System.Linq;

using Carbon.Extensions;
using Carbon.Storage;

namespace Carbon.Packaging
{
    public sealed class Manifest : Dictionary<string, IManifestEntry>
    {
        public Manifest(IEnumerable<IBlob> blobs)
        {
            foreach (var blob in blobs)
            {
                Add(blob.Key, ManifestEntry.FromBlob(blob));
            }
        }

        public Manifest(IEnumerable<IManifestEntry> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            
            foreach (var item in items)
            {
                Add(item.Key, item);
            }
        }

        public bool TryFind(string key, out IManifestEntry item)
        {
            #region Preconditions

            if (key == null) throw new ArgumentNullException(nameof(key));

            #endregion

            if (key[0] == '/')
            {
                key = key.Trim(Seperators.ForwardSlash);
            }

            return TryGetValue(key, out item);
        }

        public bool Contains(string key) => ContainsKey(key);

        public static Manifest FromPackage(Package package)
        {
            return new Manifest(package.Enumerate().Select(blob => (IManifestEntry)ManifestEntry.FromBlob(blob)));
        }
    }
}