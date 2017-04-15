using System;

using Carbon.Data.Annotations;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Data
{
    [Dataset("DatabaseBackups")]
    public class DatabaseBackup
    {
        public DatabaseBackup() { }

        public DatabaseBackup(long id, long clusterId, string name)
        {
            Id = id;
            ClusterId = clusterId;
            Name = name;
        }

        // DatabaseId + Sequence
        [Member("id"), Key]
        public long Id { get; }

        [Member("clusterId")]
        [Indexed]
        public long ClusterId { get; }

        [Member("name")]
        public string Name { get; set; }

        [Member("bucketId")]
        public long BucketId { get; set; }
        
        [Member("encryptionKeyId")]
        public long EncryptionKeyId { get; set; }

        [Member("size")]
        public long Size { get; set; }

        [Member("sha256", TypeName = "binary(32)")]
        public byte[] Sha256 { get; set; }

        // CompressionMethod...

        #region Timestamps

        [Member("started")]
        public DateTime? Started { get; set; }

        [Member("completed")]
        public DateTime? Completed { get; set; }
        
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        #endregion

        public long DatabaseId => ScopedId.GetScope(Id);
    }

    /*
    public enum CompressionMethod
    {
        None      = 0,
        GZip      = 1,
        ZStandard = 2
    }
    */
}
