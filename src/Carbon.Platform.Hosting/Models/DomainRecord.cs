using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Net.Dns;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Hosting
{
    [Dataset("DomainRecords")]
    public class DomainRecord : IResource, IDomainRecord
    {
        public DomainRecord() { }

        public DomainRecord(
            long id,
            string name,
            string path,
            DnsRecordType type, 
            string value,
            int? ttl = null, 
            DomainRecordFlags flags = default)
        {
            Validate.Id(id);
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.NotNullOrEmpty(value, nameof(value));

            #region Preconditions

            if (name.Length > 253)
                throw new ArgumentOutOfRangeException(nameof(name), name.Length, "Must be 253 characters or fewer");

            if (type == default)
                throw new ArgumentException("Required", nameof(type));

            if (ttl != null && ttl.Value < 0)
                throw new ArgumentOutOfRangeException(nameof(ttl), ttl.Value, "Must be >=0");

            #endregion

            Id    = id;
            Type  = type;
            Name  = name;
            Path  = path;
            Value = value;
            Ttl   = ttl;
            Flags = flags;
        }
        
        [Member("id"), Key] // domainId | #
        public long Id { get; }
        
        /// <summary>
        /// The name relative to the domainId
        /// e.g. @ |  subdomain
        /// </summary>
        [Member("name")]
        [Ascii, StringLength(253)]
        public string Name { get; }

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
        public DnsRecordType Type { get; }

        // A      : 192.0.2.1
        // AAAA   : 2001:0db8:85a3:0:0:8a2e:0370:7334
        // CA     : 0 issue "ca.example.net; account=123456"
        // MX     : 10 mail.example.com \ 20 mail2.example.com
        // CNAME  : hostname.example.com
        // SOA    : hostname.example.com. hostmaster.a.com. 1 7200 900 1209600 86400

        [Member("value")]
        [StringLength(4000)]
        public string Value { get; }
        
        [Member("ttl")] // in seconds (defaults to domain TTL if undefined)
        public int? Ttl { get; }
        
        // Priority / Weight?

        [Member("flags")]
        public DomainRecordFlags Flags { get; }
        
        /// <summary>
        /// The authoritive domain the record is under (DNS Zone)
        /// </summary>
        [IgnoreDataMember]
        public long DomainId => ScopedId.GetScope(Id);

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