namespace Carbon.Platform
{
	using System;
	using System.IO;

	public interface IAsset : IAssetInfo
	{
		Stream Open();
	}

	public interface IAssetInfo
	{
		string Name { get; }	// e.g. img/c.gif

		byte[] Hash { get; }	// 20 (sha1) or 32 bytes (sha256)

		DateTime Modified { get; }
	}
}