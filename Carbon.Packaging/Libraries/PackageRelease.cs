using System;
using System.Collections.Generic;

namespace Carbon.Packaging
{
    using Data;
    using Data.Annotations;
    
    // A versioned immutable instance of a package

    [Record(TableName = "PackageReleases")]
    public class PackageRelease : IPackage
    {
        public PackageRelease() { }

        public PackageRelease(IPackage package, Semver version)
        {
            Id = package.Id;
            Version = version;
        }

        [Member(1), Key] // PackageId
        public long Id { get; set; }

        [Member(2), Key] // PackageVersion
        public Semver Version { get; set; }

        [Member(3)] // May change over the life of a package... 
        public string Name { get; set; }

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

        public IList<PackageDependency> Dependencies { get; set; }

        public IList<PackageFile> Files { get; set; }

        // e.g. cropper@5.5

        // carbon.cropper

        public override string ToString() => Name + "@" + Version;
    }
}