using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Kms
{
    [Dataset("Keys", Schema = "Kms")]
    [UniqueIndex("ownerId", "name")]
    [UniqueIndex("providerId", "resourceId")]
    public class KeyInfo : IManagedResource, IKeyInfo
    {
        public KeyInfo() { }

        public KeyInfo(
            long id, 
            string name,
            long ownerId,
            ManagedResource resource,
            int version = 1,
            KeyStatus status = KeyStatus.Active)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Invalid", nameof(id));

            if (name == null || string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            if (id <= 0)
                throw new ArgumentException("Invalid", nameof(ownerId));

            #endregion

            Id         = id;
            Name       = name;
            OwnerId    = ownerId;
            Version    = version;
            Status     = status;
            ProviderId = resource.ProviderId;
            ResourceId = resource.ResourceId;
            LocationId = resource.LocationId;            
        }

        [Member("id"), Key(sequenceName: "keyId")]
        public long Id { get; }

        [Member("ownerId")]
        public long OwnerId { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }

        [Member("version")]
        public int Version { get; }

        [Member("status")]
        public KeyStatus Status { get; set; }

        #region Stats

        [Member("dekCount")]
        public int DekCount { get; }

        #endregion

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        [IgnoreDataMember]
        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.Key;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}

// master keying material 
// derive DEKs

// Google KMS: CryptoKey
// $0.06 per key per month
// stored within a "KeyRing"

// Azure: HSM Protected keys
// $1/m

// The actual key never leaves the device...

// CryptoKey ?

/*
aws   | KMS       | https://aws.amazon.com/kms/
azure | Key Vault | https://azure.microsoft.com/en-us/services/key-vault/
gcp   | KMS       | https://cloud.google.com/kms/
*/
