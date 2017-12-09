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

        public CreateEnvironmentRequest(long ownerId, string name, JsonObject properties = null)
        {
            Validate.Id(ownerId, nameof(ownerId));
            Validate.NotNullOrEmpty(name, nameof(name));

            OwnerId    = ownerId;
            Name       = name;
            Properties = properties;
        }

        [DataMember(Name = "ownerId", Order = 1)]
        public long OwnerId { get; set; }

        [DataMember(Name = "name", Order = 2)]
        [MinLength(1), StringLength(63)]
        public string Name { get; set; }

        [DataMember(Name = "properties", Order = 3)]
        public JsonObject Properties { get; set; }
    }
}