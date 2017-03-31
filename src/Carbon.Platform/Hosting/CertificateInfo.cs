using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Certificates
{
    [Dataset("Certificates")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class CertificateInfo : ICertificate, ICloudResource
    {
        [Member("id"), Key]
        public long Id { get; }
        
        [Member("subjects")]
        public string[] Subjects { get; set; }

        [Member("expires")]
        public DateTime Expires { get; set; }

        [Member("issued")]
        public DateTime? Issued { get; set; }

        [Member("revoked"), Mutable]
        public DateTime? Revoked { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        #region IResource

        // e.g. Let's Encrypt, Amazon, ...
        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        ResourceType ICloudResource.Type => ResourceType.Certificate;

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