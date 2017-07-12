using System;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Json;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Storage
{
    [Dataset("DatabaseBackups")]
    public class DatabaseBackup
    {
        public DatabaseBackup() { }

        public DatabaseBackup(long id, long bucketId, string name, JsonObject properties = null)
        {
            Id         = id;
            BucketId   = bucketId;
            Name       = name;
            Properties = properties;
        }

        // databaseId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("bucketId")]
        public long BucketId { get; }

        [Member("name")]
        [StringLength(100)]
        public string Name { get; }
        
        // the key used to protect the backup
        [Member("keyId")]
        public Uid? KeyId { get; set; }

        [Member("size")]
        public long Size { get; set; }

        [Member("sha256"), FixedSize(32)]
        public byte[] Sha256 { get; set; }

        // CompressionMethod...

        [Member("locationId")]
        public int LocationId { get; set; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

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
}
