using System;
using System.ComponentModel.DataAnnotations;

using Carbon.Net.Dns;

namespace Carbon.Platform.Hosting
{
    public class CreateDomainRecordRequest
    {
        public CreateDomainRecordRequest() { }

        public CreateDomainRecordRequest(
            long domainId,
            string name, 
            DnsRecordType type, 
            string value, 
            TimeSpan? ttl)
        {
            Validate.Id(domainId, nameof(domainId));
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.NotNullOrEmpty(value, nameof(value));

            DomainId = domainId;
            Name     = name;
            Type     = type;
            Value    = value;
            Ttl      = ttl;
        }

        public long DomainId { get; set; }

        [Required] // examples: @, www
        public string Name { get; set; }

        public DnsRecordType Type { get; set; }

        [Required]
        public string Value { get; set; }
        
        public TimeSpan? Ttl { get; set; }
    }
}