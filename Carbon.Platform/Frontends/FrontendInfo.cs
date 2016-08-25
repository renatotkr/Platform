namespace Carbon.Platform
{
    using Data.Annotations;

    [Record(TableName = "Frontends")]
    public class FrontendInfo : IFrontend
    {
        [Member(1), Identity]
        public long Id { get; set; }
        
        [Member(2, mutable: true)] // highmark
        public Semver Version { get; set; }

        [Member(3, mutable: true), Unique]
        public string Name { get; set; } // e.g. lefty

        [Member(4)]
        public long RepositoryId { get; set; }

        [Member(5, MaxLength = 40)] // Commit or named tag
        public string Revision { get; set; }

        [Member(7)]
        public long BackendId { get; set; }
    }
}
