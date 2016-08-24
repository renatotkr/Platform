using System;

namespace Carbon.Platform
{
    using Data;
    using Data.Annotations;

    [Record(TableName = "FrontendReleases")]
    public class FrontendRelease : IFrontend
    {
        [Member(1), Key] // frontendId
        public long Id { get; set; }

        [Member(2), Key]
        public Semver Version { get; set; }

        [Member(3)]
        public string Name { get; set; }

        [Member(5)]
        public long RepositoryId { get; set; }

        [Member(6, MaxLength = 40)] // Commit or named tag
        public string Revision { get; set; }

        [Member(7)] // packageHash
        public CryptographicHash Hash { get; set; }

        [Member(11)]
        public long CreatorId { get; set; }

        [Member(12), Timestamp(false)]
        public DateTime Created { get; set; }

        public override string ToString() => Name + "@" + Version.ToString();  // lefty@1.0.2
    }
}