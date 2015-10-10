namespace Carbon.Platform
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Threading.Tasks;

	public class Bundle : Collection<IAsset>
	{
		public Bundle() { }

		public Bundle(IEnumerable<IAsset> sourceFiles)
		{
			foreach (var file in sourceFiles)
			{
				Items.Add(file);
			}
		}

		public MemoryStream ToMemoryStream()
		{
			var ms = new MemoryStream();

			Save(ms);

			ms.Position = 0;

			return ms;
		}

		public void Save(Stream outputStream)
		{
			foreach (var file in Items)
			{
				using (var fileStream = file.Open())
				{
					fileStream.CopyTo(outputStream);
				}
			}
		}

		public async Task SaveAsync(string fileName, bool overwrite = false)
		{
			var targetFile = new FileInfo(fileName);

			if (targetFile.Exists)
			{
				if (!overwrite)
				{
					throw new IOException("File already exists: " + targetFile.FullName);
				}

				targetFile.Delete();
			}

			using (var combinedFile = targetFile.Create())
			{
				foreach (var file in Items)
				{
					using (var stream = file.Open())
					{
						await stream.CopyToAsync(combinedFile).ConfigureAwait(false);
					}
				}
			}
		}

	}
}
