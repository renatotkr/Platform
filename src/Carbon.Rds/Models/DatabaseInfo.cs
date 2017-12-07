using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Rds
{
    [Dataset("Databases", Schema = "Rds")]
    [UniqueIndex("ownerId", "name")]
    public class DatabaseInfo : IDatabaseInfo
    {
        public DatabaseInfo() { }

        public DatabaseInfo(
            long id,
            string name, 
            long ownerId,
            JsonObject properties = null)
        {
            Validate.Id(id);
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.Id(ownerId, nameof(ownerId));

            Id = id;
            Name       = name;
            OwnerId    = ownerId;
            Properties = properties;
        }

        [Member("id"), Key(sequenceName: "databaseId")]
        public long Id { get; }

        [Member("ownerId")]
        public long OwnerId { get; }

        [Member("name")]
        [StringLength(1, 63)]
        public string Name { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region Counters

        [Member("backupCount")]
        public int BackupCount { get; }

        [Member("endpointCount")]
        public int EndpointCount { get; }

        [Member("clusterCount")]
        public int ClusterCount { get; }

        [Member("grantCount")]
        public int GrantCount { get; }

        [Member("instanceCount")]
        public int InstanceCount { get; }

        [Member("migrationCount")]
        public int MigrationCount { get; }

        [Member("schemaCount")]
        public int SchemaCount { get; }

        [Member("userCount")]
        public int UserCount { get; }
     
        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Database;

        #endregion
    }
}