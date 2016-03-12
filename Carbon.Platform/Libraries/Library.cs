using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Carbon.Platform
{
    using Data;

    [Table("Libraries")]
    public class Library
    {
        public Library() { }

        public Library(string name, Semver version)
        {
            Name = name;
            Version = version;
        }

        [Column("name"), Key]
        [Required]
        [DataMember(Name = "name")]
        public string Name { get; set; }
        
        // e.g. 2.1.1 OR 2.1.x
        [Column("version"), Key]
        [DataMember(Name = "version")]
        public Semver Version { get; set; }
      
        [Column("isLatest")]
        [Index("isLatest-index")]
        [IgnoreDataMember]
        public int? IsLatest { get; set; }

        [Column("sha256")]
        [Index("sha256-index")]
        [DataMember(Name = "sha256", EmitDefaultValue = false)]
        public byte[] Sha256 { get; set; }

        [Column("source")]
        [DataMember(Name = "source", EmitDefaultValue = false)]
        public string Source { get; set; }
        
        [Column("path")]
        [DataMember(Name = "path")]
        public string Path { get; set; }

        [Column("created")]
        [IgnoreDataMember]
        public DateTime Created { get; set; }

        [Column("deployerId")]
        [IgnoreDataMember]
        public int? DeployerId { get; set; }

        [Column("files")]
        [DataMember(Name = "files", EmitDefaultValue = false)]
        public string[] Files { get; set; }

        [IgnoreDataMember]
        public Library[] Dependencies { get; set; }

        [IgnoreDataMember]
        internal bool IsResolved { get; set; }

        public override string ToString() => Name + "@" + Version;
    }
}