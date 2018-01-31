using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using Carbon.Data.Protection;
using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Kms.Services
{
    public class CreateKeyRequest
    {
        public CreateKeyRequest() { }

        public CreateKeyRequest(
            Uid id, 
            KeyType type,
            string name,
            byte[] data,
            KeyDataFormat format,
            Uid kekId,
            JsonObject aad,
            JsonObject properties = null)
        {
            Id         = id;
            Type       = type;
            Name       = name;
            Data       = data;
            Format     = format;
            KekId      = kekId;
            Aad        = aad;
            Properties = properties;
        }

        [DataMember(Name = "id")]
        public Uid Id { get; set; }

        [DataMember(Name = "type")]
        public KeyType Type { get; set; }

        [DataMember(Name = "name")]
        [Required, StringLength(100)]
        public string Name { get; set; }

        [DataMember(Name = "data")]
        [Required]
        public byte[] Data { get; set; }

        [DataMember(Name = "format")]
        public KeyDataFormat Format { get; set; }

        [DataMember(Name = "kekId")]
        public Uid KekId { get; set; }

        [DataMember(Name = "aad")]
        public JsonObject Aad { get; set; }

        [DataMember(Name = "properties")]
        public JsonObject Properties { get; set; }
    }
}

// Maintain JSON serializablity