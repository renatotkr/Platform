namespace Carbon.Platform
{
    using Data;
    using Data.Annotations;

    [Record(TableName = "FrontendReleases")]
    public class FrontendRelease
    {
        [Member(1), Key]
        public long FrontendId { get; set; }

        [Member(2), Key]
        public Semver Version { get; set; }

        [Member(3)]
        public long RepositoryId { get; set; }

        [Member(4, MaxLength = 40)] // Commit or named tag
        public string Revision { get; set; }

        [Member(5)]
        public CryptographicHash Signature { get; set; }

        [Member(6)]
        public long CreatorId { get; set; }
    }

    // 1/1.0.2
}