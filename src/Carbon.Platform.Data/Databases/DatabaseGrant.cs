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

        public DatabaseGrant(
            long id,
            long userId, 
            string schemaName, 
            string tableName, 
            string[] actions)
        {
            Id       = id;
            UserId   = userId;
            Resource = schemaName + "/" + tableName ?? "*";
            Actions  = actions;
        }

        // databaseId + grantId
        [Member("id"), Key]
        public long Id { get; }

        // the principal
        [Member("userId")]
        public long UserId { get; }

        // schemaName:tableName
        [Member("resource")]
        public string Resource { get; }

        [Member("actions")]
        public string[] Actions { get; }

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