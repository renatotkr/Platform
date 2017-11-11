﻿using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.CI
{
    [Dataset("RepositoryBranches", Schema = CiadDb.Name)]
    [UniqueIndex("repositoryId", "name")]
    public class RepositoryBranch : IRepositoryBranch, IResource
    {
        public RepositoryBranch() { }

        public RepositoryBranch(
            long id,
            long repositoryId,
            string name, 
            long creatorId,
            long? containerId = null)
        {
            Validate.Id(id);
            Validate.Id(repositoryId, nameof(repositoryId));
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.Id(creatorId, nameof(creatorId));

            Id           = id;
            RepositoryId = repositoryId;
            Name         = name;
            CreatorId    = creatorId;
            ContainerId  = containerId;
        }

        // repositoryId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("repositoryId")]
        public long RepositoryId { get; }

        // github limit = 255 bytes
        [Member("name")]
        [StringLength(180)]
        public string Name { get; }

        [Member("commitId"), Mutable] // latestCommitId
        public long CommitId { get; set; }

        [Member("creatorId")]
        public long CreatorId { get; }

        [Member("containerId")]
        public long? ContainerId { get; }

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

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.RepositoryBranch;
        
        #endregion
    }
}