using System.Collections.Generic;

namespace Carbon.Packaging
{
    using Data.Annotations;

    public class PackageDependency : IPackage
    {
        [Member(1), Key]
        public long PackageId { get; set; }

        [Member(2), Key]
        public SemanticVersion PackageVersion { get; set; }

        [Member(3), Key] // Lookup name...
        public long DependencyId { get; set; }

        [Member(4)]
        public SemanticVersion DependencyVersion { get; set; } 

        public IPackage ResolvedPackage { get; set; }

        #region IPackage

        long IPackage.Id => ResolvedPackage.Id;

        string IPackage.Name => ResolvedPackage.Name;

        SemanticVersion IPackage.Version => ResolvedPackage.Version;

        IList<PackageDependency> IPackage.Dependencies => ResolvedPackage.Dependencies;

        #endregion
    }
}