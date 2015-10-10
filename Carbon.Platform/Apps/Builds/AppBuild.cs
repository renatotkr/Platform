namespace Carbon.Platform
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AppBuilds")]
    public class AppBuild : IBuild
    {
        [Column("appId"), Key]
        public int AppId { get; set; }

        [Column("id"), Key]
        public Guid Id { get; set; }

        [Column("commit")]
        public string Commit { get; set; }

        [Column("status")]
        public BuildStatus Status { get; set; }

        [Column("started")]
        public DateTime? Started { get; set; }

        [Column("completed")]
        public DateTime? Completed { get; set; }
    }

    // An app may be built off a commit or tag (named commit)
}