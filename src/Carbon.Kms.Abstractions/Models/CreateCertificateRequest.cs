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
            #region Preconditions

            if (ownerId <= 0)
            {
                throw new ArgumentException("Must be > 0", nameof(ownerId));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            else if (data.Length == 0)
            {
                throw new ArgumentException("Must not be empty", nameof(data));
            }
            #endregion

            OwnerId   = ownerId;
            Data      = data;
            ChainData = chainData;
            Subjects  = subjects;
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