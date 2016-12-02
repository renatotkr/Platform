using System;

namespace Carbon.Platform.Computing
{
    using Data.Annotations;
    using Protection;
    using Versioning;

    [Dataset("ProgramReleases")]
    public class ProgramRelease : IProgram
    {
        public ProgramRelease() { }

        public ProgramRelease(long id, SemanticVersion version)
        {
            Id      = id;
            Version = version;
        }

        [Member("id"), Key]
        public long Id { get; set; }

        [Member("version"), Key] // latest
        public SemanticVersion Version { get; set; }

        [Member("name")]
        [StringLength(50)]
        public string Name { get; set; }

        // BuildId?
        [Member("buildId")]
        public long BuildId { get; set; }

        [Member("digest")]
        public Hash Digest { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        // name@1.2.1
        public override string ToString() => Name + "@" + Version;
    }
}