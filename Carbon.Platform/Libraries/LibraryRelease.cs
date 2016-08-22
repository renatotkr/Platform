using System;
using System.Collections.Generic;

namespace Carbon.Platform
{
    using Data;
    using Data.Annotations;

    [Record(TableName = "LibraryReleases")]
    public class LibraryRelease : ILibrary
    {
        public LibraryRelease() { }

        public LibraryRelease(ILibrary library, Semver version)
        {
            LibraryId = library.Id;
            Version = version;
        }

        [Member(1), Key]
        public long LibraryId { get; set; }

        [Member(2), Key]
        public Semver Version { get; set; }

        [Member(3)]
        public string LibrarySlug { get; set; }

        [Member(4)]
        public long RepositoryId { get; }

        [Member(5, MaxLength = 40)] // Commit or named tag
        public string Revision { get; set; }

        [Member(6), Unique]
        public CryptographicHash Hash { get; set; }

        [Member(7)]
        public long CreatorId { get; set; }

        [Member(8), Timestamp(false)]
        public DateTime Created { get; set; }

        #region Maps

        [Exclude]
        public IList<LibraryDepedency> Dependencies { get; set; }

        [Exclude]
        public IList<LibraryPackageFile> Files { get; set; }

        #endregion


        #region ILibrary

        long ILibrary.Id => LibraryId;

        string ILibrary.Slug => LibrarySlug;
        
        #endregion

        public override string ToString() => LibrarySlug + "@" + Version;
    }  
}