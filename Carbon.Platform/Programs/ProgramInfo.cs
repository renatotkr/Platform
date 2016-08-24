using System;
using System.Collections.Generic;

namespace Carbon.Platform
{
    using Data.Annotations;
    using Networking;

    [Record(TableName = "Programs")]
    public class ProgramInfo : IProgram
    {
        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2, mutable: true)] // highmark
        public Semver Version { get; set; }

        [Member(3), Unique]
        public string Name { get; set; }

        [Member(4)]
        public ProgramType Type { get; set; }

        [Member(5)]
        public long RepositoryId { get; set; }

        [Member(6, MaxLength = 40)] // Commit or named tag
        public string Revision { get; set; }

        [Member(8, mutable: true)] // when would a programs ports change?
        public NetworkPortList Ports { get; set; }

        [Member(11)]
        public long CreatorId { get; set; }

        [Member(12), Timestamp(false)]
        public DateTime Created { get; set; }

        // 7: Hash

        public IList<ProgramRelease> Releases { get; set; }
    }
}