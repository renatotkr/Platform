using System;

namespace Carbon.Packaging
{
    using Protection;
    using Storage;

    public class ManifestEntry
    {
        public ManifestEntry(string name, Hash hash, DateTime modified)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (name.Length == 0)
                throw new ArgumentException("Must not be empty", nameof(name));

            #endregion

            Name = name;
            Hash = hash;
            Modified = modified;
        }

        public string Name { get; }

        public DateTime Modified { get; }

        public Hash Hash { get; }

        public static ManifestEntry FromBlob(IBlob blob)
        {
            var hash = Hash.Compute(HashType.SHA256, blob.Open(), leaveOpen: false);

            return new ManifestEntry(
                name     : blob.Name,
                modified : blob.Modified,
                hash     : hash
            );
        }
    }
}
