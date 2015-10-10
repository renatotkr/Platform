namespace Carbon.Platform
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.IO;

	using Carbon.Data;

	// Consider renaming PackageManifest

	public class SiteManifest : Dictionary<string, IAssetInfo>
	{
		public SiteManifest(IEnumerable<IAssetInfo> files)
		{
			foreach (var file in files)
			{
				Add(file.Name, file);
			}
		}

		public IAssetInfo Find(string name)
		{
			name = name.Trim('/');

			IAssetInfo item;

			this.TryGetValue(name, out item);

			return item;
		}

		public bool Contains(string name)
		{
			return ContainsKey(name);
		}

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
					Convert.ToBase64String(file.Hash), 
					new XDate(file.Modified).ToIsoString()
				));
			}

			return sb.ToString();
		}

		public static SiteManifest Parse(string text)
		{
			var files = new List<IAssetInfo>();

			string line;

			using(var reader = new StringReader(text))
			{
				while ((line = reader.ReadLine()) != null)
				{
					var parts = line.Split(' ');

					files.Add(new AssetInfo(
						name	 : parts[0],
						hash	 : Convert.FromBase64String(parts[1]),
						modified : XDate.Parse(parts[2]).ToDateTime()
					));
				}
			}

			return new SiteManifest(files);
		}

		public static SiteManifest FromPackage(Package package)
		{
			return new SiteManifest(package.Select(file => (IAssetInfo)new AssetInfo(file)));
		}
	}

	public struct AssetInfo : IAssetInfo
	{
		private readonly string name;
		private readonly DateTime modified;
		private readonly byte[] hash;

		public AssetInfo(IAsset file)
		{
			using (var stream = file.Open())
			{
				this.hash = Carbon.Platform.Hash.ComputeSHA256(stream).Data;
			}

			this.name = file.Name;
			this.modified = file.Modified;
		}

		public AssetInfo(string name, byte[] hash, DateTime modified)
		{
			#region Preconditions

			if (name == null) throw new ArgumentNullException("name");

			if (name.Length == 0) throw new ArgumentException("Must not be empty", "name");

			#endregion

			this.name = name;
			this.hash = hash;
			this.modified = modified;
		}

		public string Name
		{
			get { return name; }
		}

		public byte[] Hash
		{
			get { return hash; }
		}

		public DateTime Modified
		{
			get { return modified; }
		}
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