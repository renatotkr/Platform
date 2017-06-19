using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Kms
{
    [Dataset("Secrets", Schema = "Kms")]
    [UniqueIndex("ownerId", "name")]
    public class SecretInfo
    {
        public SecretInfo() { }

        public SecretInfo(
            string name,
            long ownerId,
            string keyId,
            int keyVersion,
            byte[] iv, 
            byte[] ciphertext,
            DateTime? expires = null)
        {
            #region Preconditions

            if (ownerId <= 0)
                throw new ArgumentException("Invalid", nameof(ownerId));

            if (keyId == null)
                throw new ArgumentNullException(nameof(keyId));

            if (name == null || string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));
            
            if (iv == null || iv.Length == 0)
                throw new ArgumentException("Required", nameof(iv));

            if (ciphertext == null || ciphertext.Length == 0)
                throw new ArgumentException("Required", nameof(ciphertext));

            #endregion

            OwnerId    = ownerId;
            Name       = name;
            KeyId      = keyId;
            KeyVersion = keyVersion;
            IV         = iv;
            Ciphertext = ciphertext;
            Expires    = expires;
        }

        [Member("id"), Key(sequenceName: "secretId")]
        public long Id { get; set; }

        [Member("name")]
        [StringLength(180)]
        public string Name { get; }

        [Member("keyId")]
        public string KeyId { get; }

        [Member("keyVersion"), Mutable]
        public int KeyVersion { get; set; }

        [Member("iv"), FixedSize(16), Mutable]
        public byte[] IV { get; set; }

        [Member("ciphertext"), MaxLength(2000), Mutable]
        public byte[] Ciphertext { get; set; }
        
        [Member("ownerId")]
        public long OwnerId { get; }
        
        [Member("accessed")]
        public DateTime? Accessed { get; }

        [Member("expires"), Mutable]
        public DateTime? Expires { get; }

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }
        
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}
