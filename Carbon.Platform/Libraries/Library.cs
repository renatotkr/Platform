namespace Carbon.Libraries
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Carbon.Platform;
    using Carbon.Data;
    
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
        public string Name { get; set; }
        
        // e.g. 2.1.1 OR 2.1.x
        [Column("version"), Key]
        public Semver Version { get; set; }
      
        [Column("isLatest")]
        [Index("isLatest-index")]
        public int? IsLatest { get; set; }

        [Column("sha256")]
        [Index("sha256-index")]
        public byte[] Sha256 { get; set; }

        // TODO: Package SHA1

        [Column("source")]
        public string Source { get; set; }
        
        [Column("path")]
        public string Path { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("deployerId")]
        public int? DeployerId { get; set; }

        // Read from package.json
        public Library[] Dependencies { get; set; }

        internal bool IsResolved { get; set; }

        public override string ToString() => Name + "@" + Version;
    }
}