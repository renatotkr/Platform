using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    [Dataset("EncryptionKeys")]
    [UniqueIndex("providerId", "resourceId")]
    public class EncryptionKeyInfo : IEncryptionKey
    {
        public EncryptionKeyInfo() { }

        public EncryptionKeyInfo(
            long id, 
            string name,
            int ownerId,
            ManagedResource resource,
            int version = 1)
        {
            Id         = id;
            Name       = name;
            OwnerId    = ownerId;
            Version    = version;
            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
            ResourceId = resource.ResourceId;
        }

        [Member("id"), Key(sequenceName: "encryptionKeyId")]
        public long Id { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }

        [Member("version")]
        public int Version { get; }

        [Member("ownerId")]
        public long OwnerId { get; }

        [Member("nextRotation")]
        public DateTime? NextRotation { get; set; }
        
        #region IResource

        // Providers: aws, azure, gcp

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

        ResourceType IResource.ResourceType => ResourceTypes.EncryptionKey;

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