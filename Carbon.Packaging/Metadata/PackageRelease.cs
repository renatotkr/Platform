using System;
using System.Collections.Generic;

namespace Carbon.Packaging
{
    using Data;
    using Data.Annotations;
    
    // A versioned immutable instance of a package
    // TODO: Check that members line up exactly

    [Record(TableName = "PackageReleases")]
    public class PackageRelease : PackageBase, IPackage
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

        public override string ToString() => Name + "@" + Version;


    }
}