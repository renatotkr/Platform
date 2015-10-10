namespace Carbon.Platform
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Diagnostics;
	using System.Runtime.Serialization;

	using Carbon;
	using Carbon.Data;

	[Table("AppInstanceReports")]
	public class AppInstanceReport : IReport
	{
		[Column("iid"), Key]					// AppId + MachineId
		public long AppInstanceId { get; set; }

		[Column("rid"), Key] 
		public long Id { get; set; }

		[Column("rr")]
		public float RequestRate { get; set; }

		[Column("er")]
		public float ErrorRate { get; set; }

		[Column("trc")]
		public long TotalRequestCount { get; set; }

		#region Helpers

		[IgnoreDataMember]
		public DateRange Period
		{
			get { return ReportIdentity.Create(Id).ToDateRange(); }
			set { Id = ReportIdentity.Create(value).Value; }
		}

		#endregion

		public static AppInstanceReport FromObservations(AppInstanceObservation one, AppInstanceObservation two)
		{
			return new AppInstanceReport {
				AppInstanceId		= one.AppInstance.GetId(),
				Period				= new DateRange(one.Date, two.Date),
				RequestRate			= CounterSample.Calculate(one.RequestRateSample, two.RequestRateSample),
				ErrorRate			= CounterSample.Calculate(one.ErrorRateSample, two.ErrorRateSample),
				// SendRate			= CounterSample.Calculate(one.SendRateSample, two.SendRateSample),
				// ReceiveRate		= CounterSample.Calculate(one.ReceiveRateSample, two.ReceiveRateSample),
				TotalRequestCount	= two.TotalRequestsSample.RawValue,
				// Uptime = TimeSpan.FromSeconds(two.UptimeSample.RawValue) // TODO: Double check precision of uptimeSample
			};
		}
	}
}
