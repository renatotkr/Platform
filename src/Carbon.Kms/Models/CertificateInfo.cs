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
            int providerId,
            string resourceId,
            long? issuerId = null,
            JsonObject properties = null)
        {
            #region Preconditions

            if (subject == null)
                throw new ArgumentNullException(nameof(subject));

            if (string.IsNullOrEmpty(subject))
                throw new ArgumentException("Required", nameof(subject));

            #endregion

            Id         = id;           
            Subject    = subject;
            Properties = properties ?? new JsonObject();
            IssuerId   = issuerId;
            ResourceId = resourceId;
            ProviderId = providerId;

            // LocationId = resource.LocationId;
        }

        [Member("id"), Key]
        public Uid Id { get; }

        [Member("subject"), Indexed]
        [StringLength(100)]
        public string Subject { get; }
        
        /*
        [Member("subjectAlternateNames")]
        [StringLength(1000)]
        public string[] SubjectAlternateNames { get; }
        */
        
        // self issued if null
        [Member("issuerId")]
        public long? IssuerId { get; }

        #region Key Material

        // x509 (PFX | PEM)
        
        // public key?

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
        public DateTime? Expires { get; }

        [Member("issued")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Issued { get; }

        [Member("revoked"), Mutable]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Revoked { get; }

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }

    public static class CertificateProperties
    {
        // RSA_2048 | RSA_1024 | EC_prime256v1

        public const string KeyAlgorithm = "keyAlgorithm";
    }
}