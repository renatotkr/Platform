using System;
using System.Collections.Generic;

namespace Carbon.Computing
{
    using Data.Annotations;
    using Protection;
    using Repositories;

    [Dataset("Programs")] //  VersionTable = "ProgramReleases")]
    public class Program : IProgram
    {
        public Program() { }

        public Program(long id, SemanticVersion version)
        {
            Id      = id;
            Version = version;
        }

        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2), Version] // latest
        public SemanticVersion Version { get; set; }

        [Member(3), Unique]
        [StringLength(50)]
        public string Name { get; set; }

        [Member(4)]
        public ProgramType Type { get; set; }

        [Member(5), Mutable]
        public RepositoryInfo Source { get; set; }

        [Member(7), Mutable]
        public Hash Hash { get; set; } // signature?

        [Member(8), Mutable]
        public List<NetworkPort> Listeners { get; set; }

        [Member(12), Timestamp]
        public DateTime Created { get; set; }

        // ProgramReleases
        public IList<Program> Releases { get; set; }

        // name@1.2.1
        public override string ToString() => Name + "@" + Version;
    }

}