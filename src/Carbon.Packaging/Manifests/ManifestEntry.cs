using System;

using Carbon.Data.Protection;

namespace Carbon.Packaging
{
    using Storage;

    public class ManifestEntry : IManifestEntry
    {
        public ManifestEntry(string path, Hash hash, DateTime modified)
        {
            #region Preconditions

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (path.Length == 0)
                throw new ArgumentException("Must not be empty", nameof(path));

            #endregion

            Path = path;
            Hash = hash;
            Modified = modified;
        }

        public string Path { get; }

        public DateTime Modified { get; }

        public Hash Hash { get; }

        public static ManifestEntry FromBlob(IBlob blob)
        {
            var hash = Hash.Compute(HashType.SHA256, blob.OpenAsync().Result, leaveOpen: false);

            return new ManifestEntry(
                path     : blob.Name,
                modified : blob.Modified,
                hash     : hash
            );
        }
    }
}
