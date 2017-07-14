using System;
using Carbon.Data.Annotations;

namespace Carbon.Rds
{
    [Dataset("DatabaseGrants", Schema = "Rds")]
    [UniqueIndex("databaseId", "userId", "resource")]
    public class DatabaseGrant
    {
        public DatabaseGrant(long id, long databaseId, long userId, string resource, string[] actions)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (databaseId <= 0)
                throw new ArgumentException("Must be > 0", nameof(databaseId));

            if (userId <= 0)
                throw new ArgumentException("Must be > 0", nameof(userId));

            #endregion

            DatabaseId = databaseId;
            UserId     = userId;
            Resource   = resource ?? throw new ArgumentNullException(nameof(resource));
            Actions    = actions ?? throw new ArgumentNullException(nameof(actions));
        }

        // databaseId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("databaseId")]
        public long DatabaseId { get; }

        [Member("userId")]
        public long UserId { get; }

        [Member("resource")]
        [StringLength(100)]
        public string Resource { get; set; }

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
