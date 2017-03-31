using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Frontends
{
    using Data.Annotations;
    using Protection;
    using Versioning;
    
    [Dataset("Frontends")]
    public class Frontend : IFrontend
    {
        public Frontend() { }

        public Frontend(long id, string name)
        {
            Id   = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [Member("id"), Key] 
        public long Id { get; set; }

        [Member("version"), Mutable] // Released version
        public SemanticVersion Version { get; set; }

        // e.g. portfolio
        // e.g. portfolio#beta (branches other than master)

        [Member("name"), Unique]
        [StringLength(50)]
        public string Name { get; set; }

        [Member("source"), Mutable]  // e.g. carbon/repo#commit|head
        [StringLength(100)]
        public string Source { get; set; }

        [Member("digest")]
        public Hash Digest { get; set; }

        [Member("appId")]
        public long? AppId { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }
        
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; set; }

        public override string ToString()
        {
            // carbon@1.0.2
            // carbon#beta@1.0.2

            return Name + "@" + Version;  
        }
    }
}