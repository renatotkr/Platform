using System;
using System.Collections.Generic;

namespace Carbon.Packaging
{
    using Data;
    using Data.Annotations;

    [Record(TableName = "LibraryReleases")]
    public class LibraryRelease : IPackage
    {
        public LibraryRelease() { }

        public LibraryRelease(Library library, Semver version)
        {
            LibraryId = library.Id;
            Version = version;
        }

        [Member(1), Key]
        public long LibraryId { get; set; }

        [Member(2), Key]
        public Semver Version { get; set; }

        [Member(3)] // Indexed?
        public string LibraryName { get; set; }

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

        public IList<LibraryDepedency> Dependencies { get; set; }

        public IList<LibraryPackageFile> Files { get; set; }

        #endregion

        #region IPackage

        long IPackage.Id => LibraryId;

        string IPackage.Name => LibraryName;
        
        #endregion

        // e.g. cropper@5.5

        public override string ToString() => LibraryName + "@" + Version;
    }  
}