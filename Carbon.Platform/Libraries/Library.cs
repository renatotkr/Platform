namespace Carbon.Libraries
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public interface ILibrary
    {
        Semver Version { get; }

        LibraryRelease[] Dependencies { get; }
    }

    [Table("Libraries")]
    public class Library 
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        // github.com/carbonmade/kits/cropper
        [Column("repositoryUrl")]
        public string RepositoryUrl { get; set; }

        [Column("version")]
        public Semver Version { get; set; }
    }

    // cmcdn.net/libs/core/1.1.0/core.js

    [Table("LibraryReleases")]
    public class LibraryRelease : ILibrary
    {
        [Column("name"), Key]
        public string Name { get; set; }

        // e.g. 2.1.1 OR 2.1.x
        [Column("version"), Key]
        public Semver Version { get; set; }

        // The cdn url the library was deployed too
        // e.g. https://cmcdn.net/core/1.1.1/core.js
        [Column("url")]
        public Uri Url { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        // Read from package.json
        // TODO: Nested JSON
        public LibraryRelease[] Dependencies { get; set; }

        public bool IsResolved { get; set; }
    }
}