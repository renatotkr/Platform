using System.Collections.Generic;

namespace Carbon.Platform
{
    using Data.Annotations;

    [Record(TableName = "Programs")]
    public class ProgramInfo : IProgram
    {
        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2), Unique]
        public string Slug { get; set; }

        [Member(3)]
        public ProgramType Type { get; set; }

        [Member(4)]
        public long RepositoryId { get; set; }

        [Member(5, Mutable = true)] // highmark
        public Semver Version { get; set; }

        [Exclude]
        public IList<ProgramRelease> Releases { get; set; } // TODO

        // TODO: Ports & Permissions
    }
}