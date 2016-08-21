namespace Carbon.Platform
{
    using Data.Annotations;

    [Record(TableName = "Frontends")]
    public class FrontendInfo
    {
        [Identity]
        public long Id { get; set; }

        [Unique]
        public string Slug { get; set; } // e.g. lefty
        
        [Mutable]
        public Semver Version { get; set; }

        public long BackendId { get; set; }

        public long RepositoryId { get; set; }
    }
}
