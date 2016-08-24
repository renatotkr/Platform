using System;

namespace Carbon.Packaging
{
    using Data.Annotations;

    [Record(TableName = "Libraries")]
    public class Library : IPackage
    {
        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2), Unique]
        public string Name { get; set; }
        
        [Member(3, mutable: true)]
        public Semver Version { get; set; }

        [Member(4)]
        public long RepositoryId { get; }

        [Member(5), Timestamp]
        public DateTime Modified { get; set; }

        [Member(6), Timestamp(false)]
        public DateTime Created { get; set; }
    }

    // Libraries are just a type of package...
}