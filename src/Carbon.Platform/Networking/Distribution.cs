using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    [Dataset("Distributions")]
    [UniqueIndex("providerId", "resourceId")]
    public class Distribution : IManagedResource
    {
        public Distribution() { }

        public Distribution(
            long id, 
            long ownerId,
            long environmentId,
            int providerId, 
            string resourceId,
            JsonObject properties = null)
        {
            Validate.Id(id);
            Validate.Id(ownerId,                nameof(ownerId));
            Validate.Id(environmentId,          nameof(environmentId));
            Validate.Id(providerId,             nameof(providerId));
            Validate.NotNullOrEmpty(resourceId, nameof(resourceId));

            Id            = id;
            OwnerId       = ownerId;
            EnvironmentId = environmentId;
            Properties    = properties;
            ProviderId    = providerId;
            ResourceId    = resourceId;
        }

        [Member("id"), Key("distributionId")]
        public long Id { get; }

        [Member("ownerId"), Indexed]
        public long OwnerId { get; }

        [Member("environmentId"), Indexed]
        public long EnvironmentId { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }
        
        #region IResource

        // e.g. Borg, Cloudflare, AWS:Cloudfront, GCore

        [Member("providerId")]
        public int ProviderId { get; }

        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }
        
        ResourceType IResource.ResourceType => ResourceTypes.Distribution;

        int IManagedResource.LocationId => 0;

        #endregion

        #region Timestamps

        // provisioning until activated...
        [Member("activated")]
        public DateTime? Activated { get; }

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}

// Azure       : Endpoint
// GCore       : Resource
// Cloudfront  : Distribution
// Clouldflare : Zone