using System;

namespace Carbon.Platform.Frontends
{
    using Data.Annotations;
    using Versioning;

    [Dataset("Frontends")]
    public class Frontend : IFrontend
    {
        [Member("id"), Key] 
        public long Id { get; set; }

        [Member("version"), Version] // Current Active Version
        public SemanticVersion Version { get; set; }

        [Member("name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Member("source"), Mutable]  // e.g. carbonmade/lefty#commit
        [StringLength(100)]
        public string Source { get; }

        [Member("backendId")]
        public long BackendId { get; set; }

        [Member("creatorId")]
        public long CreatorId { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        public override string ToString() => Name + "@" + Version;  // lefty@1.0.2
    }
}