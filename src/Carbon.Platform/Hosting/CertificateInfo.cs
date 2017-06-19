using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Hosting
{
    [Dataset("Certificates")]
    [UniqueIndex("providerId", "resourceId")]
    public class CertificateInfo : ICertificate
    {
        public CertificateInfo() { }

        public CertificateInfo(
            long id, 
            string[] subjects,
            long issuerId,
            ManagedResource resource)
        {
            Id         = id;
            Subjects   = subjects ?? throw new ArgumentNullException(nameof(subjects));
            IssuerId   = issuerId;
            ProviderId = resource.ProviderId;
            ResourceId = resource.ResourceId;
            LocationId = resource.LocationId;
        }

        [Member("id"), Key(sequenceName: "certificateId")]
        public long Id { get; }
        
        // e.g. carbon.net,*.accelerator.net

        // primary subject is first, followed by subjectAlternate names
        [Member("subjects")]
        [StringLength(1000)]
        public string[] Subjects { get; }

        // KeyAlgorithm
        // RSA_2048 | RSA_1024 | EC_prime256v1

        // Not, may be different from the resource provider managing the certificate
        [Member("issuerId")]
        public long IssuerId { get; }

        // TEMP
        [Member("revocationReason")]
        public byte? RevocationReason { get; set; }

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

        ResourceType IResource.ResourceType => ResourceTypes.Certificate;

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

    //  PENDING_VALIDATION | ISSUED | INACTIVE | EXPIRED | VALIDATION_TIMED_OUT | REVOKED | FAILED

}


// [Column("keyAlgorithm")]
//  public string KeyAlgorithm { get; set; }

// [Column("serialNumber")]
// public string SerialNumber { get; set; }

// Validity (may be in future)

// VersionNumber
// SerialNumber
// RSA_2048
// OwnerId