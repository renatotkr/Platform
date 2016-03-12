namespace Carbon.Platform
{
	using System;
	using System.IO;

	public interface IAsset
	{
        string Name { get; }

        DateTime Modified { get; }

        Stream Open();
	}

	public interface IAssetInfo
	{
		string Name { get; }    // e.g. img/c.gif

        DateTime Modified { get; }

        byte[] Hash { get; }    // 20 (sha1) or 32 bytes (sha256)
    }
}