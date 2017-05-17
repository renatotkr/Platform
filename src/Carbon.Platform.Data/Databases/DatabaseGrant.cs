using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Storage
{
    [Dataset("DatabaseGrants")]
    public class DatabaseGrant
    {
        public DatabaseGrant() { }

        public DatabaseGrant(long id, long userId, string schemaName, string tableName, string[] permissions)
        {
            Id          = id;
            UserId      = userId;
            SchemaName  = schemaName;
            TableName   = tableName;
            Permissions = permissions;
        }

        // databaseId + grantId
        [Member("id"), Key]
        public long Id { get; }

        // the principal
        [Member("userId")]
        public long UserId { get; }

        [Member("schemaName")]
        [StringLength(100)]
        public string SchemaName { get; }

        [Member("tableName")]
        [StringLength(100)]
        public string TableName { get; }

        [Member("permissions")]
        public string[] Permissions { get; }

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        #endregion

        public long DatabaseId => ScopedId.GetScope(Id);
    }
}