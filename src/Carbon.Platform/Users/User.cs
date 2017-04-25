using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Users
{
    [Dataset("Users")]
    public class User
    {
        public User() { }

        public User(long id, string name)
        {
            Id = id;
            Name = name;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("name"), Unique]
        [StringLength(100)]
        public string Name { get; }

        // Unique Key...

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("created"), Timestamp(true)]
        public DateTime? Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        #endregion
    }
}
