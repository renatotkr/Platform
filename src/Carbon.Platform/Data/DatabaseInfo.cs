using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Data
{
    [Dataset("Databases")]
    public class DatabaseInfo : IDatabaseInfo
    {
        public DatabaseInfo() { }

        public DatabaseInfo(long id, string name)
        {
            Id   = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [Member("id"), Key(sequenceName: "databaseId")]
        public long Id { get; }

        [Member("name"), Unique]
        [StringLength(1, 63)]
        public string Name { get; }

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