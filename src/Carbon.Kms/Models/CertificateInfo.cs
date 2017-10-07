using System;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Kms
{
    [Dataset("Certificates")]
    // [UniqueIndex("ownerId", "name")]
    public class CertificateInfo : ICertificate //, IResource
    {
        public CertificateInfo() { }

        public CertificateInfo(
            long id,
            string name,
            long ownerId,
            long issuerId,
            byte[] data,
            CertificateDataFormat format,
            byte[] chainData = null,
            string resourceId = null,
            JsonObject properties = null)
        {
            #region Preconditions
            
            if (ownerId <= 0)
                throw new ArgumentException("Must be > 0", nameof(ownerId));
            
            if (format == default)
                throw new ArgumentException("Required", nameof(format));
            
            #endregion

            Id         = id;
            Name       = name;
            OwnerId    = ownerId;
            IssuerId   = issuerId;
            Format     = format;
            Data       = data;
            ChainData  = chainData;
            Properties = properties ?? new JsonObject();
        }

        [Member("id"), Key(sequenceName: "certificateId")]
        public long Id { get; }

        // Uid?

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }

        [Member("ownerId"), Indexed]
        public long OwnerId { get; }

        [Member("issuerId")]
        public long IssuerId { get; }

        #region Data 

        // - Subject    
        // - Issuer     
        // - Public Key (2048-bit RSA public key)
        // - Signature

        [Member("format")]
        public CertificateDataFormat Format { get; } // x509 

        /// <summary>
        /// x509v3 encoded certificate document (der, .crt extension)
        /// </summary>
        [Member("data"), MaxLength(32768)]
        public byte[] Data { get; }

        // public long ChainId { get; }

        [Member("chainData"), MaxLength(32768 * 4)]
        public byte[] ChainData { get; }

        [Member("privateKeyId")] 
        public Uid? PrivateKeyId { get; }

        #endregion

        [Member("properties")]
        [StringLength(2000)]
        public JsonObject Properties { get; }

        #region Timestamps

        [Member("expires")]
        public DateTime Expires { get; }

        [Member("issued")]
        public DateTime Issued { get; }

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