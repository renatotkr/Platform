using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Data.Protection;
using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Kms
{
    [Dataset("Keys", Schema = "Kms")]
    [DataIndex(IndexFlags.Secondary, "ownerId", "name")]
    public class KeyInfo : IKeyInfo
    {
        public KeyInfo() { }

        #region Master Key Constructor

        public KeyInfo(
           Uid id,
           long ownerId,
           string name,
           int locationId,
           string resourceId,
           KeyType type)
        {
            Id         = id;
            OwnerId    = ownerId;
            Name       = name;
            LocationId = locationId;
            ResourceId = resourceId;
            Activated  = DateTime.UtcNow;
            Status     = KeyStatus.Active;
            Type       = type;
        }

        #endregion

        public KeyInfo(
            Uid id,
            long ownerId,
            string name,
            KeyDataFormat format,
            byte[] data,
            JsonObject aad,
            Uid kekId,
            DateTime? activated = null,
            KeyType type = KeyType.Secret,
            KeyStatus status = KeyStatus.Active)
        {
            Ensure.IsValidId(ownerId, nameof(ownerId));
            Ensure.NotNullOrEmpty(name, nameof(name));
            Ensure.NotNullOrEmpty(data, nameof(data));

            Id        = id;
            OwnerId   = ownerId;
            Name      = name;
            Format    = format;
            Data      = data;
            Aad       = aad;
            KekId     = kekId;
            Activated = activated;
            Status    = status;
        }

        [Member("id"), Key]
        public Uid Id { get; }

        [Member("ownerId")]
        public long OwnerId { get; }
        
        [Member("name")]
        [StringLength(100)] // 63?
        public string Name { get; }

        // master, public, private, secret
        [Member("type")]
        public KeyType Type { get; }

        [Member("format")]
        public KeyDataFormat Format { get; }

        [Member("data")]
        [MaxLength(2500)]
        public byte[] Data { get; }
        
        [Member("kekId")]
        public Uid? KekId { get; }

        // kid, scope, subject, etc
        [Member("aad"), Optional]
        [StringLength(1000)]
        public JsonObject Aad { get; }
        
        [Member("status")]
        public KeyStatus Status { get; }

        [Member("properties"), Optional]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region IResource

        [Member("locationId")]
        public int LocationId { get; }

        [Member("resourceId")]
        [StringLength(100)]
        public string ResourceId { get; }

        #endregion

        #region Timestamps

        [Member("activated")]
        public DateTime? Activated { get; }

        [Member("accessed")]
        public DateTime? Accessed { get; }

        [Member("expires")]
        public DateTime? Expires { get; }

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        #endregion

        #region IKeyInfo

        string IKeyInfo.Id => Id.ToString();

        #endregion
    }
}