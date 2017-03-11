using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Certificates
{
    [Dataset("Certificates")]
    [DataIndex(IndexFlags.Unique, "providerId", "name")]
    public class CertificateInfo
    {
        [Member("id"), Identity]
        public long Id { get; }

        // Let's Encrypt, Amazon, ...
        [Member("providerId")]
        public int ProviderId { get; set; }

        [Member("name")]
        public string Name { get; set; }

        [Member("expires")] 
        public DateTime? Expires { get; set; }

        [Member("revoked"), Mutable]
        public DateTime? Revoked { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; set; }
    }

    //  PENDING_VALIDATION | ISSUED | INACTIVE | EXPIRED | VALIDATION_TIMED_OUT | REVOKED | FAILED

}

// [Member("hosts")] // Subjects?
// public string[] Hosts { get; set; }

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
