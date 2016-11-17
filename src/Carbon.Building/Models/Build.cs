using System;

namespace Carbon.Building
{
    using Data.Annotations;

    public class Build : IBuild
    {
        [Member(1)]
        public long Id { get; set; }

        [Member(2)]
        public BuildStatus Status { get; set; }

        [Member(3)]
        public long RepositoryId { get; set; }

        [Member(4)] // a commit or tag (named commit)
        public string Revision { get; set; }

        [Member(6), Mutable]
        public DateTime? Started { get; set; }

        [Member(7), Mutable]
        public DateTime? Completed { get; set; }

        [Member(11)]
        public long? CreatorId { get; set; }

        [Member(12), Timestamp]
        public DateTime Created { get; set; }
    }
}