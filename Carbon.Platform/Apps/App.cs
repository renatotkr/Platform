namespace Carbon.Platform
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Runtime.Serialization;
	using System.Linq;

	using Carbon.Data;

	[Table("Apps")]
	public class App : IApp
	{
		[Column("id"), Key]
		public int Id { get; set; }

		[Column("name")]
		[Index("name-index")] 
		public string Name { get; set; }

        [Column("slug")]
        [Index("slug-index")]
        public string Slug { get; set; }

        [Column("type")]
		public AppType Type { get; set; }

		[Column("repoUrl")]
		public Uri RepositoryUrl { get; set; }

		// We always move forward but don't necessarily release all versions
		// This number always moves forward
		[Column("version")]
		public int Version { get; set; }

		// The current activated version (release)
		[Column("activeVersion")]
		public int ActiveVersion { get; set; }
	
		#region Configuration

		[IgnoreDataMember]
		public bool AutoStart
		{
			get { return true; }
		}

		#endregion

		[Column("bindings")]
		[IgnoreDataMember]
		public string[] BindingSpecs { get; set; }

		[IgnoreDataMember]
		public IList<AppVersion> Versions { get; set; }

		[IgnoreDataMember]	// Domains
		[DataMember(Name = "bindings")]
		public IList<BindingInfo> Bindings
		{
			get
			{
				return (BindingSpecs == null) 
					? new BindingInfo[0]
					: BindingSpecs.Select(BindingInfo.Parse).ToArray();
			}
		}

		// Configuration Details

		// Load Balancer
	}

	// carbonmade.com			// production
	// carbonmade.com-dev		// dev
}

// App Host Type (IIS, Self, Service)