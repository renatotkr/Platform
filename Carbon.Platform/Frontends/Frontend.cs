namespace Carbon.Platform
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Frontends")]
    public class Frontend : IFrontend
    {
        [Column("id")]
        public int Id { get; set; }

        [Key, Column("name")]
        public string Name { get; set; }

        [Column("type")]
        public FrontendType Type { get; set; }

        [Column("repoUrl")]
        public Uri RepositoryUrl { get; set; }

        [Column("version")]
        public int Version { get; set; }

        [Column("activeVersion")]
        public int ActiveVersion { get; set; }

        [Column("appId")]                               // A frontend to an app
        public int? AppId { get; set; }

        [Column("ownerId")]
        public int OwnerId { get; set; }
    }

    public enum FrontendType
    {
        Unknown = 0,
        Site = 1,
        Theme = 2
    }
}