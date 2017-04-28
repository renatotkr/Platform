using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Users
{
    // A user may have mutiple identies (i.e. IAM user)

    [Dataset("Users")]
    public class User : IResource
    {
        public User() { }

        public User(long id, string name)
        {
            Id = id;
            Name = name;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        [StringLength(100)]
        public string Name { get; }

        #region IResource

        public ResourceType ResourceType => ResourceType.User;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime? Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        #endregion
    }
}
