using System;
using System.Collections.Generic;

namespace Carbon.Packaging
{
    using Data;
    using Data.Annotations;

    [Record(TableName = "PackageReleases")]
    public class PackageRelease : IPackage
    {
        public PackageRelease() { }

        public PackageRelease(IPackage package, Semver version)
        {
            PackageId = package.Id;
            Version = version;
        }

        [Member(1), Key]
        public long PackageId { get; set; }

        [Member(2), Key]
        public Semver Version { get; set; }

        [Member(3)]
        public string PackageName { get; set; }

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

        public IList<PackageDependency> Dependencies { get; set; }

        public IList<PackageFile> Files { get; set; }

        #endregion

        #region IPackage

        long IPackage.Id => PackageId;

        string IPackage.Name => PackageName;
        
        #endregion

        // e.g. cropper@5.5

        // carbon.cropper

        public override string ToString() => PackageName + "@" + Version;
    }
}