using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    [Dataset("Databases")]
    [UniqueIndex("ownerId", "name")]
    public class DatabaseInfo : IDatabaseInfo
    {
        public DatabaseInfo() { }

        public DatabaseInfo(
            long id,
            string name, 
            long ownerId)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            #endregion

            Id   = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            OwnerId = ownerId;
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

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        #endregion

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Database;

        #endregion
    }
}