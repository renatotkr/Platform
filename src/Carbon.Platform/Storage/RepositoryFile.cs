using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Storage
{
    // {branchId}/scripts/app.js

    [Dataset("RepositoryFiles", Schema = "Storage")]
    public class RepositoryFile : IRepositoryFile
    {
        public RepositoryFile() { }

        public RepositoryFile(
            long branchId, 
            string path, 
            long creatorId = 0,
            long size = 0,
            byte[] sha256 = null,
            int version = 1,
            FileType type = FileType.Blob)
        {
            #region Preconditions

            if (branchId <= 0)
                throw new ArgumentException("Must be > 0", nameof(branchId));

            #endregion

            BranchId  = branchId;
            Path      = path ?? throw new ArgumentNullException(nameof(path));
            Version   = version;
            Type      = type;
            CreatorId = creatorId;
            Size      = size;
            Sha256    = sha256;
        }
        
        [Member("branchId"), Key]
        public long BranchId { get; }
        
        [Member("path"), Key]
        [StringLength(185)] // git limit = 4096
        public string Path { get; }

        [Member("version"), Mutable]
        public int Version { get; set; }

        [Member("type")]
        public FileType Type { get; }

        // TODO: Change to Uid
        // [Member("blobId"), Mutable]
        // public long? BlobId { get; set; }

        [Member("size"), Mutable]
        public long Size { get; set; }

        // of body...
        [Member("sha256"), Mutable]
        [FixedSize(32)]
        public byte[] Sha256 { get; set; }
        
        // Sha3?

        // lastModifiedBy (lsb?)

        [Member("creatorId")]
        public long CreatorId { get; }

        // LastModifiedBy

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }

    // ObjectType...

    public enum FileType : byte
    {
        Blob      = 1,
        Directory = 2
    }
}

// Provides a flattened view of all the files within a revision
// A revision may be either a tag or a branch name