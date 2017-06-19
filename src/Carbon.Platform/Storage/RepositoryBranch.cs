using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    [Dataset("RepositoryBranches", Schema = "Storage")]
    [UniqueIndex("repositoryId", "name")]
    public class RepositoryBranch : IRepositoryBranch
    {
        public RepositoryBranch() { }

        public RepositoryBranch(
            long id,
            long repositoryId,
            string name, 
            long creatorId)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (repositoryId <= 0)
                throw new ArgumentException("Must be > 0", nameof(repositoryId));

            if (creatorId <= 0)
                throw new ArgumentException("Must be > 0", nameof(creatorId));

            #endregion

            Id           = id;
            RepositoryId = repositoryId;
            Name         = name ?? throw new ArgumentNullException(nameof(name));
            CreatorId    = creatorId;
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