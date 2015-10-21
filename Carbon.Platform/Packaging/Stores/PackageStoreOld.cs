namespace Carbon.Platform
{
	using System;
	using System.IO;
	using System.IO.Compression;
	using System.Threading.Tasks;

	using Carbon.Storage;

    [Obsolete]
	public class PackageStoreOld : IPackageStore
	{
		private readonly IBlobStore blobStore;

		public PackageStoreOld(IBlobStore blobStore)
		{
			this.blobStore = blobStore;
		}

		public async Task PutAsync(string key, Package package)
		{
			#region Preconditions

			if (package == null) throw new ArgumentNullException(nameof(package));

			#endregion

			using (var ms = new MemoryStream())
			{
				await package.ZipToAsync(ms).ConfigureAwait(false);

				ms.Seek(0, SeekOrigin.Begin);

				var blob = new Blob(ms) {
					{ "Content-Type", "application/zip" }
				};

				await blobStore.Put(key, blob).ConfigureAwait(false);
			}
		}

		public async Task<Package> GetAsync(string key)
		{
			var ms = new MemoryStream();

			using (var blob = await blobStore.Get(key).ConfigureAwait(false))
			{
				await blob.CopyToAsync(ms).ConfigureAwait(false);
			}

			ms.Seek(0, SeekOrigin.Begin);

			return ZipPackage.FromStream(ms, stripFirstLevel: false);
		}

		public async Task DownloadToAsync(string key, DirectoryInfo target)
		{
			using (var ms = new MemoryStream())
			{
				using (var blob = await blobStore.Get(key).ConfigureAwait(false))
				{
					await blob.CopyToAsync(ms).ConfigureAwait(false);
				}

				ms.Seek(0, SeekOrigin.Begin);

				using (var archive = new ZipArchive(ms, ZipArchiveMode.Read))
				{
					archive.ExtractToDirectory(target.FullName);
				}
			}
		}
	}
}

// var key = tag.Path + ".zip";  (e.g. app/2.1.1.zip)