using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

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
            ManagedResource resource)
        {
            Id         = id;
            Name       = name ?? throw new ArgumentNullException(nameof(name));
            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
            ResourceId = resource.ResourceId;
        }
        
        // providerId, regionId, zoneId, sequence
        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }

        [Member("loadBalancerId")]
        public long? LoadBalancerId { get; set; }

        // Used to check the health of the backend instances
        [Member("healthCheckId")]
        public long? HealthCheckId { get; set; }

        // A template used to create new hosts for the backend
        [Member("hostTemplateId")]
        public long? HostTemplateId { get; set; }

        public long EnvironmentId => ScopedId.GetScope(Id);

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceType.HostGroup;

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