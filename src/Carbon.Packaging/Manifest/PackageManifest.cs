using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Carbon.Packaging
{
    using Data;
    using Protection;
    using Json;
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

        public bool Contains(string name) 
            => ContainsKey(name);

        public override string ToString()
        {
            var sb = new StringBuilder();

            var i = 0;

            // sha-256:1234

            // images/img.gif 2013-04-03T04:20:39.234324Z sha256-k345jkhwsgert==
            foreach (var item in this)
            {
                if (i != 0) sb.AppendLine();

                var file = item.Value;

                // Sanity check
                if (file.Name.Split(Seperators.ForwardSlash).Last()[0] == '.' || file.Name.Contains(' ')) continue;

                // {name} {modified} {hash}
                sb.Append(file.Name);
                sb.Append(" ");
                sb.Append(new XDate(file.Modified).ToIsoString());
                sb.Append(" ");
                sb.Append(file.Hash.ToString());
                
                i++;
            }

            return sb.ToString();
        }

        public static PackageManifest Parse(string text)
        {
            var entries = new List<ManifestEntry>();

            string line;

            using (var reader = new StringReader(text))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(Seperators.Space);

                    // {name} {modified} {hash}

                    entries.Add(new ManifestEntry(
                        name     : parts[0],
                        modified : IsoDate.Parse(parts[1]).ToDateTime(),
                        hash     : Hash.Parse(parts[2])
                        
                    ));
                }
            }

            return new PackageManifest(entries);
        }

        public static PackageManifest FromPackage(Package package)
            => new PackageManifest(package.Enumerate().Select(blob => ManifestEntry.FromBlob(blob)));
    }
}