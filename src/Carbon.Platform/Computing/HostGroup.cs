using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [Dataset("HostGroups")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class HostGroup : IHostGroup
    {
        public HostGroup() { }

        public HostGroup(
            long id,
            string name,
            long environmentId,
            JsonObject details,
            ManagedResource resource)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            #endregion

            Id            = id;
            Name          = name ?? throw new ArgumentNullException(nameof(name));
            EnvironmentId = environmentId;
            Details       = details;
            ProviderId    = resource.ProviderId;
            LocationId    = resource.LocationId;
            ResourceId    = resource.ResourceId;
        }
        
        // environmentId + sequenceNumber
        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        [StringLength(100)]
        public string Name { get; }

        [Member("environmentId"), Indexed]
        public long EnvironmentId { get; }
        
        // Used to check the health of the groups's instances
        [Member("healthCheckId")]
        public long? HealthCheckId { get; set; }

        // A template used to create new hosts within the group
        [Member("hostTemplateId")]
        public long? HostTemplateId { get; set; }

        // Span (Zone, Region)

        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        // {groupName}/{groupId}
        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }
        
        // Region Scoped
        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.HostGroup;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}