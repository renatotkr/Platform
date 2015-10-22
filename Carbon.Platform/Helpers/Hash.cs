namespace Carbon.Platform
{
	using System;
	using System.IO;
	using System.Security.Cryptography;
	using System.Text;

	using Carbon.Data;

	public struct Hash 
	{
		public Hash(byte[] data)
		{
			Data = data;
		}

		public byte[] Data { get; }

        public string ToHex() => HexString.FromBytes(Data);

		public static Hash ComputeSHA1(string text)
		{
			using (var algorithm = SHA1.Create())
			{
				return new Hash(algorithm.ComputeHash(Encoding.UTF8.GetBytes(text)));
			}
		}

		public static Hash ComputeSHA256(Stream stream, bool leaveOpen = false)
		{
			#region Preconditions

			if (stream == null) throw new ArgumentNullException(nameof(stream));

			#endregion

			try
			{
				using (var algorithm = SHA256.Create())
				{
					return new Hash(algorithm.ComputeHash(stream));
				}

               
			}
			finally
			{
                if (leaveOpen)
                {
                    if (stream.CanSeek) stream.Position = 0;
                }
				else
				{
					stream.Dispose();
				}
			}
		}
	}
}