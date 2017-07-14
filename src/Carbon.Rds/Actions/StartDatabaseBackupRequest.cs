﻿namespace Carbon.Rds
{
    public class StartDatabaseBackupRequest
    {
        public StartDatabaseBackupRequest(long databaseId, string schemaName, long bucketId, string name)
        {
            DatabaseId = databaseId;
            SchemaName = schemaName;
            BucketId   = bucketId;
            Name       = name;
        }

        public long DatabaseId { get; }

        // Default to * ?
        public string SchemaName { get; }

        public long BucketId { get; }
        
        public string Name { get; }
    }
}