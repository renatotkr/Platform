using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Data
{
    [Dataset("DatabaseBackups")]
    public class DatabaseBackup
    {
        public DatabaseBackup() { }

        public DatabaseBackup(long id, long clusterId)
        {
            Id = id;
            ClusterId = clusterId;
        }

        // DatabaseId + Sequence
        [Member("id")]
        public long Id { get; }

        [Member("clusterId")]
        [Indexed]
        public long ClusterId { get; }

        [Member("bucketId")]
        public long BucketId { get; set; }

        [Member("name")]
        public string Name { get; set; }
        
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
