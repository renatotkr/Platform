using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    [Dataset("DataEncryptionKeys")]
    public class DataEncryptionKeyInfo : IResource
    {
        public DataEncryptionKeyInfo() { }

        public DataEncryptionKeyInfo(long id, byte[] ciphertext, JsonObject context, long kekId)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Invalid", nameof(id));

            if (kekId <= 0)
                throw new ArgumentException("Invalid", nameof(kekId));

            #endregion

            Id         = id;
            Ciphertext = ciphertext ?? throw new ArgumentNullException(nameof(ciphertext));
            Context    = context;
            KekId      = kekId;
        }

        [Member("id"), Key(sequenceName: "dataEncryptionKeys")]
        public long Id { get; }

        // Key Encryption Key Id : The key used to decrypt the DEK data
        [Member("kekId")]
        public long KekId { get; }

        [Member("context")]
        [StringLength(500)]
        public JsonObject Context { get; }

        [Member("ciphertext", TypeName = "varbinary(1000)")]
        public byte[] Ciphertext { get; }

        #region IResource

        public ResourceType ResourceType => ResourceType.DataEncryptionKey;

        #endregion

        #region Timestamps

        [Member("expires")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Expires { get; set; }

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        #endregion
    }
}

// DEK - Data Encryption Key - The key used to encrypt the data
// KEK - Key Encryption Key 