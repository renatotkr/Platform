using System;
using System.Collections.Generic;

namespace Carbon.Programming
{
    using Data;
    using Data.Annotations;
    using Networking;

    [Dataset("Programs")]
    [Versioned(TableName = "ProgramReleases")] 
    public class Program : IProgram
    {
        public Program() { }

        public Program(long id, Semver version)
        {
            Id      = id;
            Version = version;
        }

        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2), Version] // highmark
        public Semver Version { get; set; }

        [Member(3), Unique]
        public string Name { get; set; }

        [Member(4)]
        public ProgramType Type { get; set; }

        [Member(5)]
        public long RepositoryId { get; set; }

        [Member(6), StringLength(40), Mutable]
        public string Commit { get; set; }

        [Member(7), Mutable]
        public Hash Hash { get; set; }

        [Member(8), Mutable]
        public NetworkPortList Ports { get; set; }

        [Member(11), Mutable]
        public long CreatorId { get; set; }

        [Member(12), Timestamp]
        public DateTime Created { get; set; }

        // ProgramReleases
        public IList<Program> Releases { get; set; }

        // name@1.2.1
        public override string ToString() => Name + "@" + Version;
    }

}