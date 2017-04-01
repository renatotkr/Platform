using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Repositories
{
    [Dataset("RepositoryFiles")]
    public class RepositoryFile
    {
        // e.g. 1
        [Member("repositoryId"), Key]
        public long RepositoryId { get; set; }
        
        // e.g. master
        [Member("revision"), Key]
        public string Revision { get; set; }
        
        // e.g. scripts/apps.js
        [Member("name"), Key]
        public string Name { get; set; }
        
        [Member("blobId")]
        public long BlobId { get; set; }

        [Member("creatorId")]
        public long CreatorId { get; set; }

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

// Provides a flattened view of all the files within a revision
// A revision may be either a tag or a branch name