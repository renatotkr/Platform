using System;

namespace Carbon.Platform
{
    using Data.Annotations;

    [Record(TableName = "Certificates")]
    public class Certificate
    {
        [Member(1), Identity]
        public long Id { get; }

        [Member(2)] // Subjects?
        public string[] Hosts { get; set; }

        [Member(5, mutable: true)]
        public DateTime? Expires { get; set; }

        [Member(6, mutable: true)]
        public DateTime? Revoked { get; set; }

        [Member(6)] // e.g. LetsEncrypt, Amazon, ...
        public long ProviderId { get; set; }

        public string KeyAlgorithm { get; set; }

        [Member(7)]
        public string SerialNumber { get; set; }

        [Member(12), Timestamp(false)]
        public DateTime Created { get; set; }

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