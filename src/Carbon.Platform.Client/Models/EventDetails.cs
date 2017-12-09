using System.Runtime.Serialization;

using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Platform
{
    [DataContract]
    public class EventDetails
    {
        [DataMember(Name = "properties", EmitDefaultValue = false)]
        public Uid? Id { get; set; }

        [DataMember(Name = "action", EmitDefaultValue = false)]
        public string Action { get; set; }

        [DataMember(Name = "resource", EmitDefaultValue = false)]
        public string Resource { get; set; }

        [DataMember(Name = "context", EmitDefaultValue = false)]
        public JsonObject Context { get; set; }

        [DataMember(Name = "properties", EmitDefaultValue = false)]
        public JsonObject Properties { get; set; }
    }
}
