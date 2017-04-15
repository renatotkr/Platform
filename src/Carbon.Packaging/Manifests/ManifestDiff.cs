using System.Collections.Generic;

namespace Carbon.Packaging
{
    public class ManifestDiff
    {
        private ManifestDiff(
            IReadOnlyList<IManifestEntry> added,
            IReadOnlyList<IManifestEntry> modified,
            IReadOnlyList<IManifestEntry> removed)
        {
            Added    = added;
            Modified = modified;
            Removed  = removed;
        }

        public IReadOnlyList<IManifestEntry> Added    { get; }
        public IReadOnlyList<IManifestEntry> Modified { get; }
        public IReadOnlyList<IManifestEntry> Removed  { get; } 

        /// <param name="left">The original manifest</param>
        /// <param name="right">The new manifest</param>
        public static ManifestDiff Create(Manifest left, Manifest right)
        {
            var added    = new List<IManifestEntry>();
            var modified = new List<IManifestEntry>();
            var removed  = new List<IManifestEntry>();

            foreach (var entry in left)
            {
                if (right.TryGetValue(entry.Value.Path, out var item))
                {
                    if (item.Hash != entry.Value.Hash)
                    {
                        modified.Add(entry.Value);
                    }

                    // It's the same
                }
                else
                {
                    // it was removed (from right)
                    removed.Add(entry.Value);
                }
            }

            // Check for any new files on the right
            foreach (var entry in right)
            {
                if (!left.Contains(entry.Value.Path))
                {
                    added.Add(entry.Value); // Added
                }
            }

            return new ManifestDiff(added, modified, removed);
        }
    }
}