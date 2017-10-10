using System;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Kms
{
    [Dataset("Certificates")]
    public class CertificateInfo : ICertificate // , IResource
    {
        public CertificateInfo() { }

        public CertificateInfo(
            long id,
            long ownerId,
            byte[] data,
            long issuerId,
            DateTime expires,
            byte[] chainData = null,
            string resourceId = null,
            JsonObject properties = null)
        {
            #region Preconditions

            if (id <= 0)
            {
                throw new ArgumentException("Must be > 0", nameof(id));
            }

            if (ownerId <= 0)
            {
                throw new ArgumentException("Must be > 0", nameof(ownerId));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            else if (data.Length > 32768)
            {
                throw new ArgumentException("Must be 32,768 bytes or fewer", nameof(data));
            }

            #endregion

            Id         = id;
            OwnerId    = ownerId;
            IssuerId   = issuerId;
            Data       = data;
            ChainData  = chainData;
            Expires    = expires;
            Properties = properties ?? new JsonObject();
        }

        [Member("id"), Key(sequenceName: "certificateId")]
        public long Id { get; }

        // UID?

        [Member("ownerId"), Indexed]
        public long OwnerId { get; }

        [Member("issuerId")]
        public long IssuerId { get; }

        #region Data 

        // - Subject    
        // - Issuer     
        // - Public Key (2048-bit RSA public key)
        // - Signature

        /// <summary>
        /// x509v3 encoded certificate document (der, .crt extension)
        /// </summary>
        [Member("data"), MaxLength(32768)]
        public byte[] Data { get; }

        [Member("chainData"), MaxLength(32768 * 6)]
        public byte[] ChainData { get; }

        [Member("privateKeyId")] 
        public Uid? PrivateKeyId { get; }

        // ParentId?

        #endregion

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

// https://www.ietf.org/rfc/rfc5280.txt (X.509 Public Key Infrastructure)

// serialNumber = ???
// fingerprint  = x5t#S256
// keyAlgorithm = RSA_2048 | RSA_1024 | EC_prime256v1