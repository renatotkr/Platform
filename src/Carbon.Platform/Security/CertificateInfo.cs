using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Hosting
{
    [Dataset("Certificates")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class CertificateInfo : ICertificate
    {
        public CertificateInfo() { }

        public CertificateInfo(long id, string[] subjects, ManagedResource resource)
        {
            Id = id;
            ProviderId = resource.ProviderId;
            ResourceId = resource.ResourceId;
        }

        [Member("id"), Key]
        public long Id { get; }
        
        [Member("subjects")]
        public string[] Subjects { get; }

        [Member("expires")]
        public DateTime Expires { get; set; }

        [Member("description")]
        public string Description { get; set; }

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

        ResourceType IManagedResource.ResourceType => ResourceType.Certificate;

        long IManagedResource.LocationId => 0;

        #endregion

        #region Timestamps

        [Member("issued")]
        public DateTime? Issued { get; set; }

        [Member("revoked"), Mutable]
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