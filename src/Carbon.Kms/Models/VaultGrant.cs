using System;
using System.Runtime.Serialization;
using Carbon.Data.Annotations;
using Carbon.Data.Protection;
using Carbon.Json;

namespace Carbon.Kms
{
    [Dataset("VaultGrants")]
    public class VaultGrant
    {
        public VaultGrant() { }

        public VaultGrant(
            long id,
            long? keyId,
            string name,
            KeyUsage permissions,
            long userId,
            string externalId = null)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Invalid", nameof(id));

            if (userId <= 0)
                throw new ArgumentException("Invalid", nameof(userId));
            
            #endregion

            Id          = id;
            KeyId       = keyId;
            Name        = name ?? throw new ArgumentNullException(nameof(name));
            Permissions = permissions;
            UserId      = userId;
            ResourceId  = externalId;
        }
        
        // vaultId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        [StringLength(100)]
        public string Name { get; }

        [Member("keyId"), Indexed]
        public long? KeyId { get; }

        [Member("userId"), Indexed] // principal
        public long UserId { get; }
        
        // Encrypt, Decrypt, ...
        [Member("permissions")]
        public KeyUsage Permissions { get; }

        // The context may grant access to mutiple keys...
        [Member("context"), StringLength(200)]
        public JsonObject Constraints { get; set; }

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
}