namespace Carbon.Platform
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Diagnostics;
	using System.Runtime.Serialization;

	using Carbon;
	using Carbon.Data;

	[Table("NetworkInterfaceReports")]
	public class NetworkInterfaceReport : IReport
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

		#region Helpers

		[IgnoreDataMember]
		public DateRange Period
		{
			get { return ReportIdentity.Create(ReportId).ToDateRange(); }
			set { this.ReportId = ReportIdentity.Create(value).Value; }
		}

		#endregion

		public static NetworkInterfaceReport FromObservations(NetworkInterfaceObservation o1, NetworkInterfaceObservation o2)
		{
			int receiveRate = (int)CounterSample.Calculate(o1.ReceiveRateSample, o2.ReceiveRateSample);	// bytes per second
			int sendRate = (int)CounterSample.Calculate(o1.SendRateSample, o2.SendRateSample);			// bytes per second

			return new NetworkInterfaceReport {
				MacAddress = o1.NetworkInterface.MacAddress,
				Period		= new DateRange(o1.Date, o2.Date),
				ReceiveRate = receiveRate,
				SendRate	= sendRate
			};
		}
	}
}