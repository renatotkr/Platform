using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carbon.Building
{
    [Table("Builds")]
    public class Build : IBuild
    {
        [Column("id"), Key]
        public long Id { get; set; }

        [Column("status")]
        public BuildStatus Status { get; set; }

        [Column("source")] // RepositoryInfo (url + revision)
        public string Source { get; set; }

        [Column("started")]
        public DateTime? Started { get; set; }

        [Column("completed")]
        public DateTime? Completed { get; set; }

        [Column("creatorId")]
        public long? CreatorId { get; set; }

        [Column("created"), Timestamp]
        public DateTime Created { get; set; }
    }
}