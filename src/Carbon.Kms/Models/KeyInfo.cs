using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Data.Protection;
using Carbon.Json;

namespace Carbon.Kms
{
    [Dataset("Keys", Schema = "Kms")]
    [UniqueIndex("vaultId", "name", "version")]
    public class KeyInfo : IKeyInfo
    {
        public KeyInfo() { }

        public KeyInfo(
            long id,          
            byte[] ciphertext,
            JsonObject context,
            string name,
            long kekId,
            long vaultId,
            int version      = 1,
            KeyType type     = KeyType.Secret,
            KeyStatus status = KeyStatus.Active)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Invalid", nameof(id));

            if (kekId <= 0)
                throw new ArgumentException("Invalid", nameof(kekId));

            if (ciphertext == null || ciphertext.Length == 0)
                throw new ArgumentException("Required", nameof(ciphertext));

            #endregion

            Id         = id;
            Ciphertext = ciphertext;
            Context    = context;
            KekId      = kekId;
            Name       = name ?? throw new ArgumentNullException(nameof(name));
            Version    = 1;
            Status     = status;
            ProviderId = 1;
        }

        // vaultId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("version"), Key]
        public int Version { get; }

        [Member("vaultId"), Indexed]
        public long VaultId { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }

        // public, private, secret
        [Member("type")]
        public KeyType Type { get; }

        // the key used to decrypt the ciphertext
        [Member("kekId")]
        public long KekId { get; }

        // enough to hold a 2048 bit key + wrapper
        [Member("ciphertext"), MaxLength(2500)] 
        public byte[] Ciphertext { get; }

        // kid, scope, subject, etc
        [Member("context")]
        [StringLength(1000)]
        public JsonObject Context { get; }
        
        [Member("status")]
        public KeyStatus Status { get; }

        #region IResource

        [Member("providerId")]
        public int ProviderId { get; set; }

        [Member("resourceId")]
        [StringLength(100)]
        public string ResourceId { get; set; }

        #endregion

        #region Timestamps

        [Member("activated")] // may be in future...
        public DateTime? Activated { get; set; }

        [Member("accessed")]
        public DateTime? Accessed { get; }

        [Member("expires")]
        public DateTime? Expires { get; }

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        #endregion

        #region IKeyInfo

        string IKeyInfo.Id => Id.ToString();

        public IEnumerable<KeyValuePair<string, string>> GetAuthenticatedData()
        {
            if (Context == null) yield break;

            foreach (var property in Context)
            {
                yield return new KeyValuePair<string, string>(property.Key, property.Value.ToString());
            }
            
        }
    
        #endregion
    }
}