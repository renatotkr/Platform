using System;
using System.Runtime.Serialization;
using Carbon.Data.Annotations;
using Carbon.Json;

namespace Carbon.Kms
{
    [Dataset("KeyGrants")]
    public class KeyGrant
    {
        public KeyGrant() { }

        public KeyGrant(
            long id,
            long keyId,
            string name,
            string[] actions,
            long userId,
            string externalId = null)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Invalid", nameof(id));

            if (keyId <= 0)
                throw new ArgumentException("Invalid", nameof(keyId));

            if (userId <= 0)
                throw new ArgumentException("Invalid", nameof(userId));
            
            #endregion

            Id = id;
            KeyId      = keyId;
            Name       = name;
            Actions    = actions ?? throw new ArgumentNullException(nameof(actions));
            UserId     = userId;
            ExternalId = externalId;
        }

        [Member("id"), Key("grantId")]
        public long Id { get; }

        [Member("keyId"), Indexed]
        public long KeyId { get; }

        [Member("name")]
        public string Name { get; }

        [Member("userId")]
        public long UserId { get; }

        // Principal

        // Encrypt, Decrypt, ...
        [Member("actions")]
        public string[] Actions { get; }

        [Member("context"), StringLength(200)]
        public JsonObject Context { get; set; }

        [Member("externalId")]
        public string ExternalId { get; }

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
}