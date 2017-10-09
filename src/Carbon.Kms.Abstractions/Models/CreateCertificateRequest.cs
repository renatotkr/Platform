using System;
using System.Security.Cryptography.X509Certificates;

namespace Carbon.Kms
{
    public class CreateCertificateRequest
    {
        public CreateCertificateRequest(X509Certificate2 certificate, long ownerId)
        {
            Data    = certificate.GetRawCertData();
            Expires = certificate.NotAfter;
            OwnerId = ownerId;
        }

        public CreateCertificateRequest(
            long ownerId,
            byte[] data,
            byte[] chainData,
            string[] subjects,
            long issuerId,
            DateTime expires)
        {
            if (ownerId <= 0)
                throw new ArgumentException("Must be > 0", nameof(ownerId));

            OwnerId   = ownerId;
            Data      = data ?? throw new ArgumentNullException(nameof(data));
            ChainData = chainData;
            Subjects  = subjects ?? throw new ArgumentNullException(nameof(subjects));
            IssuerId  = issuerId;
            Expires   = expires;
        }

        public long OwnerId { get; }

        public byte[] Data { get; }

        public byte[] ChainData { get; }

        public string[] Subjects { get;  }
        
        public long IssuerId { get; }

        public DateTime Expires { get; }
    }
}