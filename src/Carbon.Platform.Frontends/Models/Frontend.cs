using System;

namespace Carbon.Platform.Frontends
{
    using Data.Annotations;
    using Protection;
    using Versioning;

    [Dataset("Frontends")]
    public class Frontend : IFrontend
    {
        public Frontend() { }

        public Frontend(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [Member("id"), Identity] 
        public long Id { get; set; }

        [Member("version"), Mutable] // Active version
        public SemanticVersion Version { get; set; }

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

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; set; }

        public override string ToString()
        {
            return Name + "@" + Version;  // carbon@1.0.2
        }
    }
}