namespace Carbon.Platform
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("AppInstanceReports")]
	public class AppInstanceReport
	{
		[Column("iid"), Key] // AppId + MachineId
		public long AppInstanceId { get; set; }

		[Column("rid"), Key] 
		public long ReportId { get; set; }

		[Column("rr")]
		public float RequestRate { get; set; }

		[Column("er")]
		public float ErrorRate { get; set; }

		[Column("trc")]
		public long TotalRequestCount { get; set; }
	}
}
