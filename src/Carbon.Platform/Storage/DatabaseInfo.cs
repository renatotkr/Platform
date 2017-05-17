using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    [Dataset("Databases")]
    public class DatabaseInfo : IDatabaseInfo
    {
        public DatabaseInfo() { }

        public DatabaseInfo(long id, string name, long ownerId)
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

        [Member("name"), Unique]
        [StringLength(1, 63)]
        public string Name { get; }

        [Member("ownerId")]
        public long OwnerId { get; }

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        #endregion

        #region IResource

        ResourceType IResource.ResourceType => ResourceType.Database;

        #endregion
    }
}

// MySQL = 64
// PostgreSQL = 63 (begin with letter or underscore)