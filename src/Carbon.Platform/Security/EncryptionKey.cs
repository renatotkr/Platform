using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Security
{
    [Dataset("EncryptionKey")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class EncryptionKeyInfo : IEncryptionKeyInfo, ICloudResource
    {
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("name")]
        public string Name { get; set; }

        [Member("version")]
        public int Version { get; set; }

        [Member("nextRotation")]
        public DateTime? NextRotation { get; set; }

        [Member("locationId")]
        public long LocationId { get; set; }
        
        #region IResource

        // Providers: Amazon, Azure, Google

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        // ResourceOwnerId { get; set; }

        ResourceType ICloudResource.Type => ResourceType.EncryptionKey;

        #endregion

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }
    }


    /*
    Amazon   KMS         https://aws.amazon.com/kms/
    Azure    Key Vault   https://azure.microsoft.com/en-us/services/key-vault/
    Google   KMS         https://cloud.google.com/kms/
    */
}
