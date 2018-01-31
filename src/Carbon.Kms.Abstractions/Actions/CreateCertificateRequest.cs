using System;

namespace Carbon.Kms
{
    public class CreateCertificateRequest
    {
        public CreateCertificateRequest(
            long ownerId,
            string[] subjects, // get subjects from the certificate data?
            byte[] data,
            byte[] encryptedPrivateKey = null,
            long? parentId = null)
        {
            Ensure.IsValidId(ownerId,   nameof(ownerId));
            Ensure.NotNullOrEmpty(data, nameof(data));

            if (parentId != null && parentId <= 0)
            {
                throw new ArgumentException("Must be > 0", nameof(parentId));
            }
            
            OwnerId             = ownerId;
            Subjects            = subjects;
            Data                = data;
            ParentId            = parentId;
            EncryptedPrivateKey = encryptedPrivateKey;
        }

        public long OwnerId { get; }

        public byte[] Data { get; }

        public long? ParentId { get; }

        public string[] Subjects { get;  }

        public byte[] EncryptedPrivateKey { get; }
    }
}