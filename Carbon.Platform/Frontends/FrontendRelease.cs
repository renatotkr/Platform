using System.ComponentModel.DataAnnotations;

namespace Carbon.Platform
{
    using Data;
    using Data.Annotations;

    [Record(TableName = "FrontendReleases")]
    public class FrontendRelease
    {
        [Key]
        public long FrontendId { get; set; }

        [Key]
        public Semver Version { get; set; }

        [StringLength(40)]
        public string Commit { get; set; }

        public long RepositoryId { get; set; }

        public CryptographicHash Signature { get; set; }

        public long CreatorId { get; set; }
    }

    // 1/1.0.2
}