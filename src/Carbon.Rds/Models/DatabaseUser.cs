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
            #region Preconditions

            if (databaseId <= 0)
                throw new ArgumentException("Must be > 0", nameof(databaseId));

            if (userId <= 0)
                throw new ArgumentException("Must be > 0", nameof(userId));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            #endregion

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
