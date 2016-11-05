using System;

namespace Carbon.Platform
{
    using Data;
    using Data.Annotations;
    using Repositories;

    [Dataset("Frontends")]
    [Versioned(TableName = "FrontendReleases")]
    public class Frontend : IFrontend
    {
        [Member(1), Key] // frontendId
        public long Id { get; set; }

        [Member(2), Version]
        public Semver Version { get; set; }

        [Member(3)]
        public string Name { get; set; }

        [Member(4), Mutable]  // e.g. carbonmade/lefty
        public RepositoryInfo Repository { get; }

        [Member(5), Mutable]
        [StringLength(40)]
        public string Commit { get; set; }

        [Member(6)]
        public Hash Hash { get; set; }

        [Member(7)]
        public long BackendId { get; set; }

        [Member(11)]
        public long CreatorId { get; set; }

        [Member(12), Timestamp]
        public DateTime Created { get; set; }

        public override string ToString() => Name + "@" + Version;  // lefty@1.0.2
    }
}