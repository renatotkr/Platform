using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Iam
{
    [Dataset("Users")]
    // [DataIndex(IndexFlags.Unique, "organizationId", "name")]
    public class User : IResource
    {
        public User() { }

        public User(long id, string name, long? organizationId = null)
        {
            Id             = id;
            Name           = name ?? throw new ArgumentNullException(nameof(name));
            OrganizationId = organizationId;
        }

        [Member("id"), Key(sequenceName: "userId")]
        public long Id { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }

        [Member("organizationId")]
        public long? OrganizationId { get; }

        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        #region IResource

        public ResourceType ResourceType => ResourceTypes.User;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime? Modified { get; }

        [IgnoreDataMember]
        [Member("suspended")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Suspended { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        #endregion
    }

    public static class UserProperties
    {
        public const string Oid     = "oid"; // Directories
        public const string Uid     = "uid"; // Linux users
        public const string IamRole = "iamRole";
    }
}