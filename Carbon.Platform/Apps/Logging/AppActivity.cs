namespace Carbon.Platform
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	using Carbon.Data;

	[Table("AppLog")]
	public class AppActivity
	{
		public static readonly Sequence Sequence = new Sequence();
		
		[Column("appId"), Key]
		public int AppId { get; set; }

		[Column("id"), Key]
		public long Id { get; set; }

		[Column("type")]
		public ActivityType Type { get; set; }

		[Index("appId-machineId-index")]
		[Column("machineId")]
		public int? MachineId { get; set; }
		
		[Index("appId-appVersion-index")]
		public int? AppVersion { get; set; }

		[Column("duration")]
		public TimeSpan? Duration { get; set; }

		// Agent
		// Ip
	}

	public enum ActivityType
	{
		Built		= 1,
		Packaged	= 2,
		Deployed	= 3,
		Activated	= 4,
		Reloaded	= 5
	}
}


// Pull the specified revision from the repository
// Build the source
// Package the build
// Sign the package
// Push the package to the PackageStore (S3)
// Create a record of the package (App Version)