using System;

namespace Carbon.Platform
{
    using Data.Annotations;

    [Record(TableName = "Certificates")]
    public class Certificate
    {
        [Member(1), Identity]
        public long Id { get; }

        [Member(2)]
        public string[] Hosts { get; set; }

        [Member(4), Timestamp(false)]
        public DateTime Created { get; set; }

        [Member(5, mutable: true)]
        public DateTime? Expires { get; set; }

        [Member(6, mutable: true)]
        public DateTime? Revoked { get; set; }

        [Member(6)] // e.g. LetsEncrypt, Amazon, ...
        public long ProviderId { get; set; }

        public string KeyAlgorithm { get; set; }

        // RSA_2048
        // OwnerId
    }

    //  PENDING_VALIDATION | ISSUED | INACTIVE | EXPIRED | VALIDATION_TIMED_OUT | REVOKED | FAILED
}
