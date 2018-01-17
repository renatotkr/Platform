using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Kms
{
    [Dataset("KeyGrants")]
    public class KeyGrant
    {
        public KeyGrant() { }

        public KeyGrant(
            Uid grantId,
            Uid keyId,
            string name,
            string[] actions,
            long userId,
            JsonObject constraints,
            JsonObject properties = null,
            string externalId = null)
        {
            Ensure.NotNullOrEmpty(name, nameof(name));
            Ensure.NotNullOrEmpty(actions, nameof(actions));
            Ensure.IsValidId(userId, nameof(userId));

            if (name.Length > 100)
                throw new ArgumentException("Must be 100 characters or fewer", nameof(name));

            Id          = grantId;
            KeyId       = keyId;
            Name        = name;
            Actions     = actions;
            UserId      = userId;
            Constraints = constraints;
            Properties  = properties;
            ResourceId  = externalId;
        }
        
        [Member("id"), Key]
        public Uid Id { get; }

        [Member("keyId"), Indexed]
        public Uid KeyId { get; }

        [Member("name")]
        [StringLength(100)]
        public string Name { get; }
        
        [Member("userId"), Indexed]
        public long UserId { get; }
        
        // e.g. encrypt, decrypt, ...
        [Member("actions")]
        public string[] Actions { get; }

        // operations / permissions?

        // The context may grant access to mutiple derived keys...
        [Member("constraints"), StringLength(200)]
        public JsonObject Constraints { get; }

        [Member("properties"), StringLength(500)]
        public JsonObject Properties { get; }

        #region Resource

        [Member("resourceId")]
        [StringLength(100)]
        public string ResourceId { get; }

        [Member("providerId")]
        public int ProviderId { get; }

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }

    public enum ConstraintsMatchPolicy : byte
    {
        Exact  = 1,
        Subset = 2
    }
}