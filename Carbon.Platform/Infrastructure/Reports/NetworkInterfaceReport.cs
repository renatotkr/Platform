namespace Carbon.Platform
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("NetworkInterfaceReports")]
	public class NetworkInterfaceReport
	{
		[Column("mac"), Key]
		public string MacAddress { get; set; }

		[Column("rid"), Key]
		public long ReportId { get; set; }

		/// <summary>
		/// Bytes per second
		/// </summary>
		[Column("rr")]
		public int ReceiveRate { get; set; }

		/// <summary>
		/// Bytes per second
		/// </summary>
		[Column("sr")]
		public int SendRate { get; set; }
	}
}