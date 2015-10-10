namespace Carbon.Platform
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Runtime.Serialization;

	using Carbon;
	using Carbon.Data;
	
	[Table("MachineReports")]
	public class MachineReport : IReport
	{
		[Key, Column("mid")] 
		[DataMember(Name = "name")]
		public int MachineId { get; set; }

		[Key, Column("rid")] 
		[DataMember(Name = "id")]
		[TimePrecision(TimePrecision.Second)]
		public long ReportId { get; set; }

		[Column("ct")]
		[DataMember(Name = "cpuTime")]
		public float CpuTime { get; set; }

		[Column("mu")]
		[DataMember(Name = "memoryUsed")]
		public long MemoryUsed { get; set; }

		// Aggregate network activity in bytes per second

		[Column("rr")]
		public int ReceiveRate { get; set; }

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

		public static MachineReport FromObservations(MachineObservation one, MachineObservation two)
		{
			var processor = ProcessorReport.FromObservations(one.Processor, two.Processor);

			var memory = new MemoryObserver().Observe();

			var report = new MachineReport {
				MachineId	= Machine.GetAsync().Result.Id,
				Period		= new DateRange(one.Date, two.Date),
				CpuTime		= processor.ProcessingTime,
				MemoryUsed	= memory.Used
			};

			if (one.NetworkInterfaces != null && one.NetworkInterfaces.Length > 0)
			{
				var len = one.NetworkInterfaces.Length;

				for (int i = 0; i < len; i++)
				{
					var r = NetworkInterfaceReport.FromObservations(one.NetworkInterfaces[i], two.NetworkInterfaces[i]);

					report.SendRate		+= r.SendRate;
					report.ReceiveRate	+= r.ReceiveRate;
				}
			}

			return report;
		}
	}
}