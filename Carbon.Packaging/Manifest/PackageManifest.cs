using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Carbon.Packaging
{
    using Data;
    using Json;
    using Storage;

    public class PackageManifest : Dictionary<string, PackageFileInfo>
    {
        public PackageManifest(IEnumerable<IBlob> blobs)
        {
            foreach (var blob in blobs)
            {
                Add(blob.Name, new PackageFileInfo(blob));
            }
        }

        public PackageManifest(IEnumerable<PackageFileInfo> blobs)
        {
            foreach (var blob in blobs)
            {
                Add(blob.Name, blob);
            }

        }
        public PackageFileInfo Find(string name)
        {
            name = name.Trim('/');

            PackageFileInfo item;

            TryGetValue(name, out item);

            return item;
        }

        public bool Contains(string name) => ContainsKey(name);

        public override string ToString()
        {
            var sb = new StringBuilder();

            // images/img.gif k345jkhwsgert== 2013-04-03T04:20:39.234324Z
            foreach (var item in this)
            {
                var file = item.Value;

                // Sanity check
                if (file.Name.Split('/').Last()[0] == '.' || file.Name.Contains(' ')) continue;

                sb.AppendLine(string.Format("{0} {1} {2}",
                    file.Name,
                    Convert.ToBase64String(file.Hash.Data),
                    new XDate(file.Modified).ToIsoString()
                ));
            }

            return sb.ToString();
        }

        public static PackageManifest Parse(string text)
        {
            var blobs = new List<PackageFileInfo>();

            string line;

            using (var reader = new StringReader(text))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(' ');

                    blobs.Add(new PackageFileInfo(
                        name     : parts[0],
                        sha256   : Convert.FromBase64String(parts[1]),
                        modified : IsoDate.Parse(parts[2]).ToDateTime()
                    ));
                }
            }

            return new PackageManifest(blobs);
        }

        public static PackageManifest FromPackage(Package package)
            => new PackageManifest(package.Select(file => new PackageFileInfo(file)));
    }

    public struct PackageFileInfo
    {
        public PackageFileInfo(IBlob file)
        {
            Name     = file.Name;
            Modified = file.Modified;
            Size     = file.Size;
            Hash     = Hash.Compute(HashType.SHA256, file.Open());
        }

        public PackageFileInfo(string name, byte[] sha256, DateTime modified)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (name.Length == 0)
                throw new ArgumentException("Must not be empty", nameof(name));

            #endregion

            Name = name;
            Hash = new Hash(HashType.SHA256, sha256);
            Modified = modified;
            Size = 0;
        }

        public string Name { get; }

        public long Size { get; }

        public Hash Hash { get; }

        public DateTime Modified { get; }
    }
}