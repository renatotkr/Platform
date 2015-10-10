namespace Carbon.Platform
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("VolumeReports")]
	public class VolumeReport
	{
		[Column("vid"), Key]
		public string VolumeId { get; set; }

		[Column("rid"), Key] 
		public long ReportId { get; set; }

		/// <summary>
		/// The number of bytes available at the end of the reporting period
		/// </summary>
		[Column("a")]
		public long Available { get; set; }

		/// <summary>
		/// The capacity (in bytes) of the drive
		/// </summary>
		[Column("s")]
		public long Size { get; set; }

		/// <summary>
		/// The number of bytes used at the end of the reporting period
		/// </summary>
		[Column("u")]
		public long Used { get; set; }

		/// <summary>
		/// The number of bytes read per second from the drive during the reporting period
		/// </summary>
		[Column("rr")]
		public int ReadRate { get; set; }

		/// <summary>
		/// The number of bytes written per second to the drive during the reporting period
		/// </summary>
		[Column("wr")]
		public int WriteRate { get; set; }

		/// <summary>
		/// The percentage of total time the drive was busy servicing read operations
		/// </summary>
		[Column("rt")]
		public float ReadTime { get; set; }

		/// <summary>
		/// The percentage of total time the drive was busy servicing write operations
		/// </summary>
		[Column("wt")]
		public float WriteTime { get; set; }
	}
}