using System;
using System.Security.Cryptography;
using Carbon.Data.Annotations;
using Carbon.Json;

namespace Carbon.Kms
{
    [Dataset("Certificates")]
    public class CertificateInfo : ICertificate
    {
        public CertificateInfo() { }

        public CertificateInfo(
            long id,
            long ownerId,
            byte[] data, // der encoded
            long? parentId,
            DateTime expires,
            byte[] encryptedPrivateKey = null,
            JsonObject properties = null)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            else if (data.Length > 32768)
            {
                throw new ArgumentException("Must be 32,768 bytes or fewer", nameof(data));
            }
            
            if (encryptedPrivateKey != null && encryptedPrivateKey.Length > 2500)
            {
                throw new ArgumentException("Must be less than 2,500 bytes", nameof(encryptedPrivateKey));
            }
            
            #endregion

            Id                  = id;
            Data                = data;
            ParentId            = parentId;
            EncryptedPrivateKey = encryptedPrivateKey;
            Expires             = expires;
            IssuerId            = ownerId;
            Properties          = properties;
            
            using (var hash = SHA256.Create()) // x5t#S256
            {
                Fingerprint = hash.ComputeHash(data);
            }
        }

        [Member("id"), Key(sequenceName: "certificateId")]
        public long Id { get; }

        /// <summary>
        /// x509v3 encoded certificate document (der)
        /// </summary>
        [Member("data"), MaxLength(32768)] // Blob?
        public byte[] Data { get; }
        
        [Member("parentId"), Indexed]
        public long? ParentId { get; }
        
        // every certificate has it's own private key
        [Member("encryptedPrivateKey"), MaxLength(2500)]
        public byte[] EncryptedPrivateKey { get; }

        [Member("fingerprint"), Unique] // sha256(data)
        [FixedSize(32)]
        public byte[] Fingerprint { get; }

        [Member("issuerId")] // TODO: Rename ownerId
        public long IssuerId { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region Timestamps

        [Member("expires")]
        public DateTime Expires { get; }

        [Member("revoked"), Mutable]
        public DateTime? Revoked { get; }

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}

// NOTE: Max row size is 65535

// https://www.ietf.org/rfc/rfc5280.txt (X.509 Public Key Infrastructure)