namespace Carbon.Platform
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Runtime.Serialization;
    using System.Net;

    using Carbon.Data;

	[Table("Machines")]
	public class MachineInfo : IMachine
	{
		[Column("id"), Key]
		[DataMember(Name = "id")]
		public int Id { get; set; }

		// TODO: Change to byte

		[Column("hash")]
		[Index("hash-index")]
		[DataMember(Name = "hash")]
		public string Hash { get; set; }

		[Column("status")]
		[DataMember(Name = "status")]
		public DeviceStatus Status { get; set; }

		[Column("description")]
		[DataMember(Name = "description")]
		public string Description { get; set; }

		[Column("instanceId")]
		[DataMember(Name = "instanceId")]
		[Index("instanceId-index")]
		public string InstanceId { get; set; }

		[Column("imageId")]
		[DataMember(Name = "imageId")]
		public string ImageId { get; set; }

		[Column("instanceType")]
		[DataMember(Name = "instanceType")]
		public string InstanceType { get; set; }

		[Column("availabilityZone")]
		[DataMember(Name = "availabilityZone")]
		public string AvailabilityZone { get; set; }

		[Column("processorCount")]
		[DataMember(Name = "processorCount")]
		public int ProcessorCount { get; set; }

		[Column("volumeNames")]
		[DataMember(Name = "volumeNames")]
		public string[] VolumeNames { get; set; }

		[Column("macs")]
		[IgnoreDataMember]
		public string[] Macs { get; set; }

		[Column("ips")]
		[DataMember(Name =  "ips")]
		public string[] Ips { get; set; }

		[Column("privateIp")]
		[DataMember(Name = "privateIp")]
		public IPAddress PrivateIp { get; set; }

        [Column("heartbeat")]
        public DateTime? Heartbeat { get; set; }

		[Column("memoryTotal")]
		[IgnoreDataMember]
		public long MemoryTotal { get; set; }

		[Column("started")]
		[DataMember(Name = "started")]
		public DateTime? Started { get; set; }

		[Column("terminated")]
		[DataMember(Name = "terminated")]
		public DateTime? Terminated { get; set; }
	}
}