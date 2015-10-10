﻿namespace Carbon.Platform
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Runtime.Serialization;
	
	[Table("MachineReports")]
	public class MachineReport
    {
		[Key, Column("mid")] 
		[DataMember(Name = "name")]
		public int MachineId { get; set; }

		[Key, Column("rid")] 
		[DataMember(Name = "id")]
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
	}
}