using System;
using Carbon.Data.Annotations;
using Carbon.Security;

namespace Carbon.Rds
{
    [Dataset("DatabaseUsers", Schema = "Rds")]
    public class DatabaseUser : IUser
    {
        public DatabaseUser() { }

        public DatabaseUser(long databaseId, long userId, string name)
        {
            Validate.Id(databaseId, nameof(databaseId));
            Validate.Id(userId, nameof(userId));
            Validate.NotNullOrEmpty(name, nameof(name));

            DatabaseId = databaseId;
            UserId     = userId;
            Name       = name;
        }

        [Member("databaseId"), Key]
        public long DatabaseId { get; }

        [Member("userId"), Key]
        public long UserId { get; }

        // MySQL    : 32 characters
        // Postgres : 63 characters

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }
        
        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }
        
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
        
        #region IUser

        long IUser.Id => UserId;

        #endregion
    }
}
