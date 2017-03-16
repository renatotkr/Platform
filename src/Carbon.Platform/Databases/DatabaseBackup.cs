using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Databases
{
    [Dataset("DatabaseBackups")]
    public class DatabaseBackup
    {
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("databaseId")]
        [Indexed]
        public long DatabaseId { get; set; }

        [Member("bucketId")]
        public long BucketId { get; set; }

        // [Member]
        // public long EncryptionKey { get; set; }

        [Member("size")]
        public long Size { get; set; }

        [Member("sha256", TypeName = "binary(32)")]
        public byte[] SHA256 { get; set; }

        // CompressionMethod...

        [Member("started")]
        public DateTime? Started { get; set; }

        [Member("completed")]
        public DateTime? Completed { get; set; }
        
        [Member("created"), Timestamp]
        public DateTime Created { get; set; }
    }

    /*
    public enum CompressionMethod
    {
        None      = 0,
        GZip      = 1,
        ZStandard = 2
    }
    */

    public enum BackupType : byte
    {
        Full   = 1,
        Partial = 2
    }
}
