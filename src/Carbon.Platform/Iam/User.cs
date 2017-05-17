using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Iam
{
    [Dataset("Users")]
    public class User : IResource
    {
        public User() { }

        public User(long id, string name)
        {
            Id   = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [Member("id")]
        [Key(sequenceName: "userId")] // TODO: Carve out a small range...
        public long Id { get; }

        // scope name to directory?

        [Member("name")]
        [StringLength(63)]
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


// oid

// A user may have mutiple identities
// e.g. Iam, Borg, Google, ...

// IAM: max name = 64 characters
