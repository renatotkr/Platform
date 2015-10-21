namespace Carbon.Platform
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Runtime.Serialization;

	[Table("FrontendFiles")]
	public class FrontendFile : IAssetInfo
	{
		[Column("frontend"), Key] // {frontend}/{head}
		[Required]
		public string Frontend { get; set; }

		[Key, Column("name")]
		[Required]
		public string Name { get; set; }

		[Column("lmb")]
		public int? LastModifiedBy { get; set; }

		[Column("sha256")]
		public byte[] Sha256 { get; set; }

		[Column("modified")]
		public DateTime Modified { get; set; }

		#region Helpers

		[IgnoreDataMember]
		public byte[] Hash => Sha256;

		#endregion
	}
}
