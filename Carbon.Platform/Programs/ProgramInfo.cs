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

        [Member(2), Unique]
        public string Name { get; set; }

        [Member(3)]
        public ProgramType Type { get; set; }

        [Member(4)]
        public long RepositoryId { get; set; }

        [Member(5, mutable: true)] // highmark
        public Semver Version { get; set; }

        [Member(6, mutable: true)] // when would a programs ports change?
        public NetworkPortList Ports { get; set; }

        public IList<ProgramRelease> Releases { get; set; }
    }
}