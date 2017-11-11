using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Carbon.Json;

namespace Carbon.Platform.Environments
{
    [DataContract]
    public class CreateEnvironmentRequest
    {
        public CreateEnvironmentRequest() { }

        public CreateEnvironmentRequest(string name, long ownerId, JsonObject properties = null)
        {
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.Id(ownerId, nameof(ownerId));

            Name       = name;
            OwnerId    = ownerId;
            Properties = properties;
        }

        [DataMember(Name = "name", Order = 1)]
        [MinLength(1), StringLength(63)]
        public string Name { get; set; }

        [DataMember(Name = "ownerId", Order = 2)]
        public long OwnerId { get; set; }

        [DataMember(Name = "properties", Order = 3)]
        public JsonObject Properties { get; set; }
    }
}