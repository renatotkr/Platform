using System.Collections.Generic;

namespace Carbon.Packaging
{
    using Data.Annotations;

    // Latest release of a package...
    // Libraries are just a type of package...

    [Record(TableName = "Packages")]
    public class PackageInfo : PackageBase, IPackage
    {
        public PackageInfo() { }

        public PackageInfo(string name, Semver version)
        {
            Name = name;
            Version = version;
        }

        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2, mutable: true)]
        public Semver Version { get; set; }

        [Member(3, mutable: true), Unique] // May change
        public string Name { get; set; }

        public IList<PackageRelease> Releases { get; set; }
    }
}