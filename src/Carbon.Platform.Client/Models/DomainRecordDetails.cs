using System.Runtime.Serialization;

namespace Carbon.Platform.Computing
{
    public class DomainRecordDetails
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }
        
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}