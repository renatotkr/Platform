using System;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Json;
using Carbon.Platform.Sequences;

namespace Carbon.Rds
{
    [Dataset("DatabaseBackups", Schema = "Rds")]
    public class DatabaseBackup : IDatabaseBackup
    {
        public DatabaseBackup() { }

        public DatabaseBackup(
            long id, 
            long bucketId, 
            string name, 
            DatabaseBackupType type = DatabaseBackupType.Full,
            Uid? encryptionKeyId = null,
            JsonObject properties = null)
        {
            Validate.Id(id,               nameof(id));
            Validate.Id(bucketId,         nameof(bucketId));
            Validate.NotNullOrEmpty(name, nameof(name));

            Id         = id;
            BucketId   = bucketId;
            Name       = name;
            KeyId      = encryptionKeyId;
            Type       = type;
            Properties = properties;
        }

        // databaseId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("type")]
        public DatabaseBackupType Type { get; }

        [Member("bucketId")]
        public long BucketId { get; }

        [Member("name")]
        [StringLength(100)]
        public string Name { get; }

        [Member("status")]
        public DatabaseBackupStatus Status { get; }

        // the key used to protect the backup
        [Member("keyId")]
        public Uid? KeyId { get; }

        [Member("size")]
        public long Size { get; }

        [Member("sha256"), FixedSize(32)]
        public byte[] Sha256 { get; }
        
        [Member("message")]
        [StringLength(500)]
        public string Message { get; }

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

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion

        public long DatabaseId => ScopedId.GetScope(Id);
    }
}
