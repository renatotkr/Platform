using System;
using System.Runtime.Serialization;

namespace Carbon.Platform
{
    [DataContract]
    public class UserDetails
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }

        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "roles", EmitDefaultValue = false)]
        public string[] Roles { get; set; }

        [DataMember(Name = "created", EmitDefaultValue = false)]
        public DateTime? Created { get; set; }
    }
}