using System;

namespace Carbon.Platform.Computing
{
    using Data.Annotations;
    using Versioning;

    // Programs may be terminating tasks
    // Such, they are not named apps...

    [Dataset("Programs")]
    public class Program : IProgram
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("version")] // currently active version
        public SemanticVersion Version { get; set; }

        [Member("name"), Unique]
        [StringLength(50)]
        public string Name { get; set; }

        [Member("type")]
        public ProgramType Type { get; set; }

        // Runtime...

        [Member("source"), Mutable]
        [StringLength(100)]
        public string Source { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        // Releases...

        // name@1.2.1
        public override string ToString() => Name + "@" + Version;
    }
}