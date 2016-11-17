﻿using System;

namespace Carbon.Platform
{
    using Data.Annotations;
    using Protection;
    using Repositories;

    [Dataset("Frontends")] //  VersionTable = "FrontendReleases")]
    public class Frontend : IFrontend
    {
        [Member(1), Key] // frontendId
        public long Id { get; set; }

        [Member(2), Version]
        public SemanticVersion Version { get; set; }

        [Member(3)]
        [StringLength(50)]
        public string Name { get; set; }

        [Member(4), Mutable]  // e.g. carbonmade/lefty#commit
        public RepositoryInfo Source { get; }

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