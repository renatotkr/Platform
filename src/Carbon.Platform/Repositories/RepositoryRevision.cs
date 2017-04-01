using System;
using Carbon.Data.Annotations;
using System.Runtime.Serialization;

namespace Carbon.Platform.Repositories
{
    // AKA Reference

    [Dataset("RepositoryRevisions")]
    [DataIndex(IndexFlags.Unique, "repositoryId", "name")]
    public class RepositoryRevision : IManagedResource
    {
        [Member("repositoryId"), Key]
        public long RepositoryId { get; set; }

        [Member("name"), Key]
        public string Name { get; set; }

        [Member("type")]
        public RevisionType Type { get; set; }

        [Member("commitId")]
        public long CommitId { get; set; } // e.g. d921970aadf03b3cf0e71becdaab3147ba71cdef

        [Member("creatorId")] // aka taggerId for tags
        public long CreatorId { get; set; }

        #region Preconditions

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }
        
        // heads/fancy
        // tags/fancy
        
        // carbon/cropper#heads/master

        [IgnoreDataMember]
        [Member("resourceId")]
        public string ResourceId { get; set; }

        ResourceType IManagedResource.Type => ResourceType.RepositoryRevision;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        [Member("deleted")]
        public DateTime? Deleted { get; set; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; set; }

        #endregion
    }
}
