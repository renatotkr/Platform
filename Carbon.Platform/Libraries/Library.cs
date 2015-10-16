namespace Carbon.Libraries
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Libraries")]
    public class Library
    {
        [Column("name")]
        public string Name { get; set; }

        // github.com/carbonmade/kits/cropper
        [Column("repositoryUrl")]
        public string RepositoryUrl { get; set; }
    }
    
    [Table("LibraryReleases")]
    public class LibraryRelease
    {
        [Column("name"), Key]
        public string Name { get; set; }

        // Sequential (timestamp + unique sequence)

        [Column("id"), Key]
        public long Id { get; set; } 

        [Column("version")] // e.g. 2.1.1
        public Semver Version { get; set; }

        // The cdn url the library was deployed too
        // e.g. https://cmcdn.net/core/1.1.1/core.js
        [Column("url")]
        public Uri Url { get; set; }

        // Read from package.json
        public Dependency[] Dependencies { get; set; }
    }
}


/*
[Column("version_major")]
public int VersionMajor { get; set; }

[Column("version_minor")]
public int VersionMinor { get; set; }

[Column("version_patch")]
public int VersionPatch { get; set; }
*/