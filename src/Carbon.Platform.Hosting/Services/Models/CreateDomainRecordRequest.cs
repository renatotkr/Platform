using System;
using Carbon.Net.Dns;

namespace Carbon.Platform.Hosting
{
    public struct CreateDomainRecordRequest
    {
        public CreateDomainRecordRequest(
            long domainId,
            string name, 
            DnsRecordType type, 
            string value, 
            TimeSpan? ttl)
        {
            #region Preconditions

            if (domainId <= 0)
                throw new ArgumentException("Must be > 0", nameof(domainId));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            if (name.Length > 253)
                throw new ArgumentException("Must be less than 255 characters", nameof(name));

            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Required", nameof(value));

            if (ttl != null && ttl.Value < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(ttl), ttl.Value.TotalSeconds, "Must be >= 0");

            #endregion

            DomainId = domainId;
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
