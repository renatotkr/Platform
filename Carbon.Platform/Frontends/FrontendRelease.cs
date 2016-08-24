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

        [Member(4)]
        public long RepositoryId { get; set; }

        [Member(5, MaxLength = 40)] // Commit or named tag
        public string Revision { get; set; }

        [Member(6)] // packageHash
        public CryptographicHash Hash { get; set; }

        [Member(6)]
        public long CreatorId { get; set; }

        public override string ToString() => Name + "@" + Version.ToString();  // lefty@1.0.2
    }
}