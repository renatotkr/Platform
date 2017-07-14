using System;
using Carbon.Data.Annotations;

namespace Carbon.Rds
{
    [Dataset("DatabaseUsers", Schema = "Rds")]
    public class DatabaseUser
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

        [Member("name")]
        [StringLength(100)]
        public string Name { get; }
        
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
