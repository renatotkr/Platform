using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using Carbon.Data.Protection;
using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Kms.Services
{
    public class CreateKeyRequest
    {
        [DataMember(Name = "id")]
        public Uid Id { get; set; }

        [DataMember(Name = "name")]
        [Required, StringLength(100)]
        public string Name { get; set; }

        [DataMember(Name = "data")]
        [Required]
        public byte[] Data { get; set; }

        [DataMember(Name = "type")]
        public KeyType Type { get; set; }

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