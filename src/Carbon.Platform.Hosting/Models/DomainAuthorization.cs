using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Hosting
{
    [Dataset("DomainAuthorizations")]
    public class DomainAuthorization
    {
        public DomainAuthorization() { }

        public DomainAuthorization(
            long id,
            DomainAuthorizationType type,
            JsonObject properties,
            DateTime? completed = null,
            DateTime? expires = null,
            DomainAuthorizationFlags flags = default)
        {
            Validate.Id(id);

            Id         = id;
            Type       = type;
            Properties = properties;
            Flags      = flags;
            Completed  = completed;
            Expires    = expires;
        }

        [Member("id"), Key] // domainId | #
        public long Id { get; }

        [Member("type")]
        public DomainAuthorizationType Type { get; set; }

        [Member("properties")]
        [StringLength(2000)]
        public JsonObject Properties { get; set; }

        [Member("flags")]
        public DomainAuthorizationFlags Flags { get; }

        [Member("completed")] // null while pending | processing
        public DateTime? Completed { get; }

        [Member("expires")]
        public DateTime? Expires { get; }

        [Member("revoked")]
        public DateTime? Revoked { get; }

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }
        
        #region Helpers

        public long DomainId => ScopedId.GetScope(Id);

        public bool IsPending => Completed == null;

        public bool IsRevoked => Revoked != null;

        public bool IsExpired => Expires != null && Expires <= DateTime.UtcNow;

        public bool IsValid => Completed != null && !IsRevoked && !IsExpired;

        #endregion

        // "unknown", "pending", "processing", "valid", "invalid" and "revoked"
    }

    public enum DomainAuthorizationType
    {
        Unknown = 0,
        Acme    = 1
    }
}

/*
// ACME ----
{
    "status": "valid",
    "expires": "2015-03-01",

    "identifier": {
        "type": "dns",
        "value": "example.org"
    },

    "challenges": [
        {
            "type": "http-01",
            "status": "valid",
            "validated": "2014-12-01T12:05Z",
            "keyAuthorization": "SXQe-2XODaDxNR...vb29HhjjLPSggwiE"
        }
    ],
}
*/