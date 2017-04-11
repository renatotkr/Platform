using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.VersionControl
{
    // 1/master/scripts/app.js

    [Dataset("RepositoryFiles")]
    public class RepositoryFile
    {
        public RepositoryFile() { }

        public RepositoryFile(
            long repositoryId, 
            string branchName, 
            string path, 
            FileType type = FileType.Blob)
        {
            RepositoryId = repositoryId;
            BranchName   = branchName ?? throw new ArgumentNullException(nameof(branchName));
            Path         = path ?? throw new ArgumentNullException(nameof(path));
            Type         = type;
        }

        [Member("repositoryId"), Key]
        public long RepositoryId { get; }
        
        [Member("branchName"), Key]
        [StringLength(63)]
        public string BranchName { get; }
        
        [Member("path"), Key]
        [StringLength(255)] // git limit = 4096
        public string Path { get; }

        [Member("type")]
        public FileType Type { get; }

        [Member("blobId")]
        public long? BlobId { get; set; }

        [Member("size")]
        public long Size { get; set; }

        [Member("creatorId")]
        public long CreatorId { get; set; }

        // LastModifiedBy

        #region Hashes

        [Member("sha256", TypeName = "binary(32)")]
        public byte[] Sha256 { get; set; }

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }

    public enum FileType : byte
    {
        Blob      = 1,
        Directory = 2
    }
}

// Provides a flattened view of all the files within a revision
// A revision may be either a tag or a branch name