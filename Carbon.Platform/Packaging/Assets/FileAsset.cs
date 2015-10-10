namespace Carbon.Platform
{
	using System;
	using System.IO;

	internal class FileAsset : IAsset
	{
		private readonly FileInfo file;

		public FileAsset(string name, FileInfo file)
		{
			#region Preconditions

			if (file == null) throw new ArgumentNullException(nameof(file));

			if (!file.Exists)
			{
				throw new ArgumentException($"'{file.FullName}' does not exist.", nameof(file));
			}

			#endregion

			Name = name;

			this.file = file;
		}

		public string Name { get; }

		public DateTime Modified => file.LastWriteTimeUtc;

		public Stream Open()
		{
			return file.Open(FileMode.Open, FileAccess.Read, FileShare.Read); 
		}

		#region Hash

		byte[] IAssetInfo.Hash
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

	}
}