using System;
using System.Collections.Generic;

namespace Carbon.Packaging
{
    using Data;
    using Data.Annotations;

    public class PackageBase
    {
        // 1 = Id
        // 2 = Version
        // 3 = Name

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
    }
}
