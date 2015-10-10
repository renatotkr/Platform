namespace Carbon.Platform
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Diagnostics;
	using System.Runtime.Serialization;

	using Carbon;
	using Carbon.Data;

	[Table("VolumeReports")]
	public class VolumeReport : IReport
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

		#region Helpers

		[IgnoreDataMember]
		public DateRange Period
		{
			get { return ReportIdentity.Create(ReportId).ToDateRange(); }
			set { this.ReportId = ReportIdentity.Create(value).Value; }
		}

		#endregion

		public static VolumeReport FromObservations(VolumeObservation one, VolumeObservation two)
		{
			var report = new VolumeReport {
				VolumeId	= one.Volume.FullName,
				Period		= new DateRange(one.Date, two.Date),
				Available	= two.Available,
				Used		= two.Used,
				Size		= two.Size,
				ReadTime	= CounterSample.Calculate(one.ReadTimeSample, two.ReadTimeSample),
				WriteTime	= CounterSample.Calculate(one.WriteTimeSample, two.WriteTimeSample),
				ReadRate	= (int)CounterSample.Calculate(one.ReadRateSample, two.ReadRateSample),
				WriteRate	= (int)CounterSample.Calculate(one.WriteRateSample, two.WriteRateSample)
			};

			return report;
		}
	}
}