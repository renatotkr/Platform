using System;

using Carbon.Data.Protection;
using Carbon.Storage;

namespace Carbon.Packaging
{
    public class ManifestEntry : IManifestEntry
    {
        public ManifestEntry(string key, Hash hash, DateTime modified)
        {
            #region Preconditions

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (key.Length == 0)
                throw new ArgumentException("Must not be empty", nameof(key));

            #endregion

            Key = key;
            Hash = hash;
            Modified = modified;
        }

        public string Key { get; }

        public DateTime Modified { get; }

        public Hash Hash { get; }

        public static ManifestEntry FromBlob(IBlob blob)
        {
            var hash = Hash.Compute(HashType.SHA256, blob.OpenAsync().Result, leaveOpen: false);

            return new ManifestEntry(
                key      : blob.Name,
                modified : blob.Modified,
                hash     : hash
            );
        }
    }
}
