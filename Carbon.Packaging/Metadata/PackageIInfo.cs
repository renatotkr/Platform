using System;
using System.Collections.Generic;

namespace Carbon.Packaging
{
    using Data;
    using Data.Annotations;
    using Repositories;

    [Dataset("Packages")]
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

        [Member(2), Version]
        public Semver Version { get; set; }

        [Member(3), Mutable, Unique]
        public string Name { get; set; }

        [Member(4), Mutable]
        public RepositoryInfo Repository { get; set; }

        [Member(5), Mutable]
        [StringLength(40)]
        public string Commit { get; set; }

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