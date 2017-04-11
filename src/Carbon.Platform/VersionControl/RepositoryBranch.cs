using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.VersionControl
{
    [Dataset("RepositoryBranches")]
    [DataIndex(IndexFlags.Unique, "repositoryId", "name")]
    public class RepositoryBranch
    {
        public RepositoryBranch() { }

        public RepositoryBranch(long repositoryId, string name, long creatorId)
        {
            RepositoryId = repositoryId;
            Name         = name ?? throw new ArgumentNullException(nameof(name));
            CreatorId    = creatorId;
        }

        [Member("repositoryId"), Key]
        public long RepositoryId { get; }

        [Member("name"), Key]
        [StringLength(63)]
        public string Name { get; }

        // Changes after each commit
        [Member("commitId")]
        public long CommitId { get; set; }

        [Member("creatorId")]
        public long CreatorId { get; }

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; set; }

        #endregion
    }
}

// e.g. d921970aadf03b3cf0e71becdaab3147ba71cdef