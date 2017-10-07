using System;

namespace Carbon.Kms
{
    public class CreateCertificateRequest
    {
        public CreateCertificateRequest(
            long ownerId,
            byte[] data,
            byte[] chainData,
            string[] subjects,
            long issuerId,
            DateTime issued)
        {
            OwnerId   = ownerId;
            Data      = data;         // certificate.RawData;
            ChainData = chainData;
            Subjects  = subjects ?? throw new ArgumentNullException(nameof(subjects));
            IssuerId  = issuerId;
            Issued    = issued;
        }

        public long OwnerId { get; }

        public byte[] Data { get; set; }

        public byte[] ChainData { get; set; }

        public string[] Subjects { get; set; }
        
        public long IssuerId { get; set; }

        public DateTime Issued { get; set; }
    }
}