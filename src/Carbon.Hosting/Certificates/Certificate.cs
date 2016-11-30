using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carbon.Security
{
    [Table("Certificates")]
    public class Certificate
    {
        [Column, Key] // Identity
        public long Id { get; }

        [Column] // Subjects?
        public string[] Hosts { get; set; }

        [Column] // Mutable
        public DateTime? Expires { get; set; }

        [Column] // Mutable
        public DateTime? Revoked { get; set; }

        [Column] // e.g. LetsEncrypt, Amazon, ...
        public long ProviderId { get; set; }

        public string KeyAlgorithm { get; set; }

        [Column]
        public string SerialNumber { get; set; }

        [Column, Timestamp]
        public DateTime Created { get; set; }

        // Validity (may be in future)

        // VersionNumber
        // SerialNumber
        // RSA_2048
        // OwnerId
    }

    //  PENDING_VALIDATION | ISSUED | INACTIVE | EXPIRED | VALIDATION_TIMED_OUT | REVOKED | FAILED
}


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