using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;

namespace Carbon.Kms
{
    [Dataset("Certificates")]
    [UniqueIndex("providerId", "resourceId")]
    public class CertificateInfo // : ICertificate
    {
        public CertificateInfo() { }

        public CertificateInfo(
            Uid id,
            string name,
            string[] subjects,
            long issuerId,
            int providerId,
            string resourceId,
            int version = 1)
        {
            Id         = id;
            Name       = name ?? throw new ArgumentNullException(nameof(name));
            Subjects   = subjects ?? throw new ArgumentNullException(nameof(subjects));
            IssuerId   = issuerId;
            ProviderId = providerId;
            ResourceId = resourceId;
            // LocationId = resource.LocationId;
            Version    = version;
        }

        [Member("id"), Key]
        public Uid Id { get; }

        [Member("name")]
        [StringLength(100)]
        public string Name { get; }

        [Member("version")]
        public int Version { get; }
        
        // e.g. carbon.net,*.accelerator.net

        // primary subject is first, followed by subjectAlternate names
        [Member("subjects")]
        [StringLength(1000)]
        public string[] Subjects { get; }

        // KeyAlgorithm
        // RSA_2048 | RSA_1024 | EC_prime256v1
        
        [Member("issuerId")]
        public long IssuerId { get; }

        #region Key Material

        // x509 (PFX | PEM)

        // public EncryptedData[] PrivateKey { get; set; }

        [Member("x5t")]
        public byte[] X5t { get; set; }

        #endregion

        #region Helpers

        public string PrimarySubject => Subjects[1];

        #endregion

        #region IResource

        // e.g. Let's Encrypt, Amazon, ...
        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        [Member("resourceVersion")]
        public int ResourceVersion { get; set; }

        // ResourceType IResource.ResourceType => ResourceTypes.Certificate;

        // aws certificates are region scoped
        [IgnoreDataMember]
        [Member("locationId")]
        public int LocationId { get; }

        #endregion

        #region Timestamps

        [Member("expires")]
        public DateTime Expires { get; set; }

        [Member("issued")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Issued { get; set; }

        [Member("revoked"), Mutable]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Revoked { get; set; }

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}