using System;
using System.Collections.Generic;

namespace Carbon.Programming
{
    using Data;
    using Data.Annotations;
    using Networking;

    [Record(TableName = "Programs"), Versioned(TableName = "ProgramReleases")] 
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

        [Member(2, mutable: true), Version] // highmark
        public Semver Version { get; set; }

        [Member(3), Unique]
        public string Name { get; set; }

        [Member(4)]
        public ProgramType Type { get; set; }

        [Member(5)]
        public long RepositoryId { get; set; }

        [Member(6, MaxLength = 40)] // Commit or named tag
        public string Revision { get; set; }

        [Member(7, mutable: true)]
        public Hash Hash { get; set; }

        [Member(8, mutable: true)]
        public NetworkPortList Ports { get; set; }

        [Member(11, mutable: true)]
        public long CreatorId { get; set; }

        [Member(12), Timestamp(false)]
        public DateTime Created { get; set; }

        // ProgramReleases
        public IList<Program> Releases { get; set; }

        // name@1.2.1
        public override string ToString() => Name + "@" + Version;
    }

}