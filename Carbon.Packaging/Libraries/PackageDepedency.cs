﻿using System.Collections.Generic;

namespace Carbon.Packaging
{
    using Data.Annotations;

    public class PackageDependency : IPackage
    {
        [Member(1), Key]
        public long PackageId { get; set; }

        [Member(2), Key]
        public long DependencyId { get; set; }

        [Member(3)]
        public string DepedencyName { get; set; }

        [Member(4)]
        public Semver DependencyVersion { get; set; } 

        public IPackage ResolvedPackage { get; set; }

        #region IPackage

        long IPackage.Id => ResolvedPackage.Id;

        string IPackage.Name => ResolvedPackage.Name;

        Semver IPackage.Version => ResolvedPackage.Version;

        IList<PackageDependency> IPackage.Dependencies => ResolvedPackage.Dependencies;

        #endregion
    }
}