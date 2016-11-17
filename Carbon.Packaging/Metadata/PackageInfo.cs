using System;
using System.Collections.Generic;

namespace Carbon.Packaging
{
    using Data.Annotations;
    using Protection;
    using Repositories;

    [Dataset("Packages")]
    // [Versioned(TableName = "PackageReleases")]
    public class PackageInfo : IPackage
    {
        public PackageInfo() { }

        public PackageInfo(string name, SemanticVersion version)
        {
            Name = name;
            Version = version;
        }

        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2), Version]
        public SemanticVersion Version { get; set; }

        [Member(3), Mutable, Unique]
        [StringLength(50)]
        public string Name { get; set; }

        [Member(4), Mutable]
        public RepositoryInfo Source { get; set; }
   
        [Member(5), Unique]
        public Hash Hash { get; set; }

        [Member(6)]
        public string Main { get; set; } // TODO

        [Member(12), Timestamp]
        public DateTime Created { get; set; }

        
        public IList<PackageDependency> Dependencies { get; set; }

        public IList<PackageFile> Files { get; set; }

        public IList<PackageInfo> Releases { get; set; }

        public override string ToString() => Name + "@" + Version;
    }
}