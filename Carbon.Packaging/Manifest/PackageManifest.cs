using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Carbon.Packaging
{
    using Data;

    public class PackageManifest : Dictionary<string, IFileInfo>
    {
        public PackageManifest(IEnumerable<IFileInfo> files)
        {
            foreach (var file in files)
            {
                Add(file.Name, file);
            }
        }

        public IFileInfo Find(string name)
        {
            name = name.Trim('/');

            IFileInfo item;

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
            var files = new List<IFileInfo>();

            string line;

            using (var reader = new StringReader(text))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(' ');

                    files.Add(new AssetInfo(
                        name     : parts[0],
                        sha256   : Convert.FromBase64String(parts[1]),
                        modified : XDate.Parse(parts[2]).ToDateTime()
                    ));
                }
            }

            return new PackageManifest(files);
        }

        public static PackageManifest FromPackage(Package package)
        {
            return new PackageManifest(package.Select(file => (IFileInfo)new AssetInfo(file)));
        }
    }

    internal struct AssetInfo : IFileInfo
    {
        public AssetInfo(IFile file)
        {
            using (var stream = file.Open())
            {
                Hash = CryptographicHash.ComputeSHA256(stream);
            }

            Name = file.Name;
            Modified = file.Modified;
        }

        public AssetInfo(string name, byte[] sha256, DateTime modified)
        {
            #region Preconditions

            if (name == null) throw new ArgumentNullException(nameof(name));

            if (name.Length == 0) throw new ArgumentException("Must not be empty", nameof(name));

            #endregion

            Name = name;
            Hash = new CryptographicHash(HashAlgorithmType.SHA256, sha256);
            Modified = modified;
        }

        public string Name { get; }

        public CryptographicHash Hash { get; }

        public DateTime Modified { get; }
    }

    /*
	[Flags]
	public enum AssetType
	{
		File,
		Folder
	}
	*/
}