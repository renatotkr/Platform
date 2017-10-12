using System;
using Carbon.Data.Annotations;

using Carbon.Json;
using Carbon.Net.Dns;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Hosting
{
    [Dataset("Domains")]
    public class Domain : IDomain
    {
        public Domain() { }

        public Domain(
            long id,
            string name,
            long? ownerId = null,
            long? environmentId = null,
            string[] nameServers = null,
            long? registrationId = null,
            DomainFlags flags = default,
            JsonObject properties = null)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (ownerId != null && ownerId.Value <= 0)
                throw new ArgumentException("Must be > 0", nameof(ownerId));

            if (registrationId != null && registrationId.Value <= 0)
                throw new ArgumentException("Must be > 0", nameof(registrationId));

            if (environmentId != null && environmentId.Value <= 0)
                throw new ArgumentException("Must be > 0", nameof(environmentId));

            #endregion

            Id             = id;
            Name           = name;
            Path           = new DomainName(name.ToLower()).Path;
            OwnerId        = ownerId;
            NameServers    = nameServers;
            RegistrationId = registrationId;
            EnvironmentId  = environmentId;
            Flags          = flags;
            Properties     = properties ?? new JsonObject();
        }

        [Member("id"), Key(sequenceName: "domainId")]
        public long Id { get; }
        
        [Member("name")]
        [Ascii, StringLength(253)]
        public string Name { get; }

        // Punycoded normalized path (reverse order)
        // e.g. ai/processor
        [Member("path"), Unique]
        [Ascii, StringLength(253)]
        public string Path { get; }

        [Member("ownerId"), Indexed]
        public long? OwnerId { get; }

        [Member("environmentId"), Indexed]
        public long? EnvironmentId { get; }

        [Member("certificateId")]
        public long? CertificateId { get; }

        [Member("nameServers")]
        [Ascii, StringLength(500)]
        public string[] NameServers { get; }

        /// <summary>f
        /// If registered through a register
        /// </summary>
        [Member("registrationId")]
        public long? RegistrationId { get; }

        [Member("ttl")]
        public int? Ttl { get; }

        [Member("flags")]
        public DomainFlags Flags { get; }
        
        // { whoisServer }
        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region Counts

        // the # of times the domain has been registered
        // [Member("registrationCount")]
        // public int RegistrationCount { get; }

        [Member("recordCount")]
        public int RecordCount { get; }

        #endregion

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Domain;

        #endregion

        #region Timestamps

        [Member("validated")]
        public DateTime? Validated { get; }

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}

// A domain may serve as a Zone

// https://en.wikipedia.org/wiki/DNS_zone