using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Kms
{
    [Dataset("Certificates")]
    [UniqueIndex("providerId", "resourceId")]
    public class CertificateInfo : ICertificate
    {
        public CertificateInfo() { }

        public CertificateInfo(
            Uid id,
            long ownerId,
            string name,
            string subject,
            long issuerId,
            int providerId,
            string resourceId,
            JsonObject properties = null)
        {
            Id         = id;           
            Subject    = subject ?? throw new ArgumentNullException(nameof(subject));
            Properties = properties ?? new JsonObject();
            IssuerId   = issuerId;
            ProviderId = providerId;
            ResourceId = resourceId;
            // LocationId = resource.LocationId;
        }

        [Member("id"), Key]
        public Uid Id { get; }

        [Member("subject"), Indexed]
        [StringLength(100)]
        public string Subject { get; set; }

        /*
        [Member("subjectAlternateNames")]
        [StringLength(1000)]
        public string[] SubjectAlternateNames { get; }
        */

        [Member("issuerId")]
        public long IssuerId { get; }

        #region Key Material

        // x509 (PFX | PEM)

        #endregion

        // fingerprint  = x5t#S256
        // keyAlgorithm = RSA_2048 | RSA_1024 | EC_prime256v1

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region IResource

        // e.g. Let's Encrypt, Amazon, ...
        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("locationId")]
        public int LocationId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

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