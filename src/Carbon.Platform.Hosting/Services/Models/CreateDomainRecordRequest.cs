using System;
using Carbon.Net.Dns;

namespace Carbon.Platform.Hosting
{
    public class CreateDomainRecordRequest
    {
        public CreateDomainRecordRequest(
            long zoneId,
            string name, 
            DnsRecordType type, 
            string value, 
            TimeSpan? ttl)
        {
            #region Preconditions

            if (zoneId <= 0)
                throw new ArgumentException("Must be > 0", nameof(zoneId));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Required", nameof(value));

            if (ttl != null && ttl.Value < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(ttl), ttl.Value.TotalSeconds, "Must be >= 0");

            #endregion

            DomainId = zoneId;
            Name     = name;
            Type     = type;
            Value    = value;
            Ttl      = ttl;
        }

        public long DomainId { get; }

        // @, subdomain
        public string Name { get; }

        public DnsRecordType Type { get; }

        public string Value { get; }
        
        public TimeSpan? Ttl { get; }
    }
}
