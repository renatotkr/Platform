using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Hosting
{
    [Dataset("Certificates")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
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
        public string[] Subjects { get; }

        [Member("description")]
        public string Description { get; set; }
        
        // KeyAlgorithm
        // RSA_2048 | RSA_1024 | EC_prime256v1

        // Not, may be different from the resource provider managing the certificate
        [Member("issuerId")]
        public long IssuerId { get; }

        // TEMP
        [Member("revocationReason")]
        public byte? RevocationReason { get; set; }

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
        [TimePrecision(TimePrecision.Second)]
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

/*
Certificate
Version Number
Serial Number
Signature Algorithm ID
Issuer Name
Validity period
Not Before
Not After
Subject name
Subject Public Key Info
Public Key Algorithm
Subject Public Key
Issuer Unique Identifier (optional)
Subject Unique Identifier (optional)
Extensions (optional)
*/