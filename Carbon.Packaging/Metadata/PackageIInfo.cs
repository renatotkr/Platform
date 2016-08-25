using System;
using System.Collections.Generic;

namespace Carbon.Packaging
{
    using Data;
    using Data.Annotations;

    [Record(TableName = "Packages")]
    [Versioned(TableName = "PackageReleases")]
    public class PackageInfo : IPackage
    {
        public PackageInfo() { }

        public PackageInfo(string name, Semver version)
        {
            Name = name;
            Version = version;
        }

        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2, mutable: true), Version]
        public Semver Version { get; set; }

        [Member(3, mutable: true), Unique]
        public string Name { get; set; }

        [Member(4)]
        public long RepositoryId { get; set; }

        [Member(5, MaxLength = 40)] // Commit or named tag
        public Revision Revision { get; set; }

        [Member(6), Unique]
        public Hash Hash { get; set; }

        [Member(11)]
        public long CreatorId { get; set; }

        [Member(12), Timestamp]
        public DateTime Created { get; set; }

        public PackageFile Main { get; set; } // TODO

        public IList<PackageDependency> Dependencies { get; set; }

        public IList<PackageFile> Files { get; set; }

        public IList<PackageInfo> Releases { get; set; }

        public override string ToString() => Name + "@" + Version;
    }
}