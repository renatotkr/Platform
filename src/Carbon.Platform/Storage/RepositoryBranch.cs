﻿using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Storage
{
    [Dataset("RepositoryBranches")]
    [UniqueIndex("repositoryId", "name")]
    public class RepositoryBranch : IRepositoryBranch
    {
        public RepositoryBranch() { }

        public RepositoryBranch(
            long repositoryId,
            string name, 
            long creatorId)
        {
            #region Preconditions

            if (repositoryId == 0)
                throw new ArgumentException("Must be > 0", nameof(repositoryId));

            #endregion

            RepositoryId = repositoryId;
            Name         = name ?? throw new ArgumentNullException(nameof(name));
            CreatorId    = creatorId;
        }

        [Member("repositoryId"), Key]
        public long RepositoryId { get; }

        [Member("name"), Key]
        [StringLength(63)]
        public string Name { get; }

        [Member("creatorId")]
        public long CreatorId { get; }

        [Member("commitId"), Mutable]
        public long CommitId { get; set; }

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}

// e.g. d921970aadf03b3cf0e71becdaab3147ba71cdef