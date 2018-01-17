using System;
using Carbon.Data.Annotations;
using Carbon.Data.Sql;

namespace Carbon.Rds
{
    [Dataset("DatabaseGrants", Schema = "Rds")]
    public class DatabaseGrant : IDatabaseGrant
    {
        public DatabaseGrant() { }

        public DatabaseGrant(
            long id,
            long databaseId,
            DbObject resource,
            string[] privileges,
            long userId,
            string[] columnNames = null)
        {
            Ensure.IsValidId(id);
            Ensure.IsValidId(databaseId, nameof(databaseId));
            Ensure.IsValidId(userId, nameof(userId));
            Ensure.NotNull(privileges, nameof(privileges));

            #region Preconditions
            
            if (privileges.Length == 0)
            {
                throw new ArgumentException("Must not be empty", nameof(privileges));
            }

            #endregion

            Id          = id;
            DatabaseId  = databaseId;
            UserId      = userId;
            SchemaName  = resource.SchemaName;
            ObjectType  = resource.Type;
            ObjectName  = resource.ObjectName;
            Actions     = privileges;
            ColumnNames = columnNames;
        }

        // databaseId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("databaseId")] 
        public long DatabaseId { get; }

        [Member("userId"), Indexed]
        public long UserId { get; }

        [Member("schemaName")]
        [StringLength(63)]
        public string SchemaName { get; }
        
        [Member("objectType")]
        public DbObjectType ObjectType { get; }

        [Member("objectName")]
        [StringLength(63)]
        public string ObjectName { get; }

        [Member("columnNames")]
        [StringLength(200)]
        public string[] ColumnNames { get; }

        [Member("actions")]
        [StringLength(1000)]
        public string[] Actions { get; }
        
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

// TODO:
// [ ] Rename Actions to Privileges
// [ ] Remove databaseId column