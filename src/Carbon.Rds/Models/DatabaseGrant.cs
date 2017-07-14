using System;
using Carbon.Data.Annotations;

namespace Carbon.Rds
{
    [Dataset("DatabaseGrants", Schema = "Rds")]
    public class DatabaseGrant : IDatabaseGrant
    {
        public DatabaseGrant(
            long id,
            long databaseId, 
            long userId, 
            string schemaName,
            string tableName, 
            string[] actions)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));
            
            if (databaseId <= 0)
                throw new ArgumentException("Must be > 0", nameof(databaseId));

            if (userId <= 0)
                throw new ArgumentException("Must be > 0", nameof(userId));

            if (string.IsNullOrEmpty(schemaName))
                throw new ArgumentException("Required", nameof(schemaName));

            #endregion

            Id         = id;
            DatabaseId = databaseId;
            UserId     = userId;
            SchemaName = schemaName;
            TableName  = tableName;
            Actions    = actions ?? throw new ArgumentNullException(nameof(actions));
        }

        // databaseId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("databaseId")]
        public long DatabaseId { get; }

        [Member("userId")]
        public long UserId { get; }

        [Member("schemaName")]
        [StringLength(63)]
        public string SchemaName { get; }

        [Member("tableName")]
        [StringLength(63)]
        public string TableName { get; }

        [Member("columnNames")]
        [StringLength(200)]
        public string[] ColumnNames { get; set; }

        [Member("functionName")]
        [StringLength(63)]
        public string FunctionName { get; set; }

        // aka privileges
        [Member("actions")]
        [StringLength(1000)]
        public string[] Actions { get; set; }
        
        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}
