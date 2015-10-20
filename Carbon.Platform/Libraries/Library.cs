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
        [Column("name"), Key]
        public string Name { get; set; }
        
        // e.g. 2.1.1 OR 2.1.x
        [Column("version"), Key]
        public Semver Version { get; set; }
      
        [Column("isLatest")]
        [Index("isLatest-index")]
        public bool IsLatest { get; set; }

        [Column("sha256")]
        public byte[] Sha256 { get; set; }

        // codesource
        [Column("source")]
        public string Source { get; set; }

        // The cdn url the library was deployed too
        // e.g. https://cmcdn.net/libs/core/1.1.0/core.js
        [Column("url")]
        public Uri Url { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        // Read from package.json
        public Library[] Dependencies { get; set; }

        internal bool IsResolved { get; set; }

        public override string ToString() => Name + "@" + Version;
    }
}