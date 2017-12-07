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
            #region Preconditions
            
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            else if (data.Length == 0)
            {
                throw new ArgumentException("Must not be empty", nameof(data));
            }

            if (parentId != null && parentId <= 0)
            {
                throw new ArgumentException("Must be > 0", nameof(parentId));
            }

            #endregion

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