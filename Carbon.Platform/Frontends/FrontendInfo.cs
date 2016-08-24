namespace Carbon.Platform
{
    using Data.Annotations;

    [Record(TableName = "Frontends")]
    public class FrontendInfo
    {
        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2), Unique]
        public string Slug { get; set; } // e.g. lefty
        
        [Member(3, mutable: true)] // highmark
        public Semver Version { get; set; }

        [Member(4)]
        public long BackendId { get; set; }

        [Member(5)]
        public long RepositoryId { get; set; }
    }
}
