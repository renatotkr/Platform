using System;
using System.Linq;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Hosting
{
    [Dataset("DomainRecords")]
    public class DomainRecord : IDomain
    {
        public DomainRecord() { }

        public DomainRecord(
            long id,
            long domainId,
            string name,
            DomainRecordType type, 
            string value, 
            int? ttl = null, 
            DomainRecordFlags flags = default)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (domainId <= 0)
                throw new ArgumentException("Must be > 0", nameof(domainId));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));
            
            if (name != name.ToLower())
                throw new ArgumentException("Must be lowercase", nameof(name));

            if (type == default)
                throw new ArgumentException("Required", nameof(type));

            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Required", nameof(value));

            if (ttl != null && ttl.Value < 0)
                throw new ArgumentOutOfRangeException(nameof(ttl), ttl.Value, "Must be >=0");

            #endregion

            Id       = id;
            DomainId = domainId;
            Type     = type;
            Path     = string.Join("/", name.Split('.').Reverse());
            Value    = value;
            Ttl      = ttl;
        }
        
        [Member("id"), Key(sequenceName: "domainRecordId")]
        public long Id { get; }
        
        [Member("domainId"), Indexed]
        public long DomainId { get; }

        public string Name => string.Join(".", Path.Split('/').Reverse());

        /// <summary>
        /// Punycoded Domain:ReversedPathNotation
        /// ai/processor
        /// ai/processor/*
        /// </summary>
        [Member("path"), Indexed]
        [Ascii, StringLength(253)]
        public string Path { get; }

        /// <summary>
        /// Resource Record Type
        /// e.g. A, MX, CNAME, AAAA
        /// </summary>
        [Member("type")]
        public DomainRecordType Type { get; }

        // A      : 192.0.2.1
        // AAAA   : 2001:0db8:85a3:0:0:8a2e:0370:7334
        // CA     : 0 issue "ca.example.net; account=123456"
        // MX     : 10 mail.example.com \ 20 mail2.example.com
        // CNAME :  hostname.example.com

        [Member("value")]
        [StringLength(4000)]
        public string Value { get; }
        
        [Member("ttl")] // in seconds (defaults to domain TTL if undefined)
        public int? Ttl { get; }
        
        [Member("flags")]
        public DomainRecordFlags Flags { get; }

        // AuthoritiveDomain

        // zone / authority

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.DnsRecord;

        #endregion
    }
}

/*
{
    name  : "subdomain",
    type  : "A",
    value : "192.168.1.1" 
}
*/
