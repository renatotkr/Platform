using System;
using System.Runtime.Serialization;

namespace Carbon.Platform
{
    [DataContract]
    public class DomainRecordDetails
    {
        public DomainRecordDetails() { }

        public DomainRecordDetails(
            string name, 
            string type, 
            string value,
            int? ttl = null)
        {
            Name  = name   ?? throw new ArgumentNullException(nameof(name));
            Type  = type   ?? throw new ArgumentNullException(nameof(type));
            Value = value  ?? throw new ArgumentNullException(nameof(value));
            Ttl   = ttl;
        }

        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(Name = "type")] // DnsRecordType
        public string Type { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }

        [DataMember(Name = "ttl", EmitDefaultValue = false)]
        public int? Ttl { get; set; }
    }
}