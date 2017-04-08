using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Apps;

// What is the relationship between an application environment and backend?
// Does each backend have an environment
// Are they the same?

// accelerator#staging

namespace Carbon.Platform.Computing
{
    [Dataset("Backends")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class BackendInfo : IBackend
    {
        public BackendInfo() { }

        public BackendInfo(long id, string name, IAppEnvironment env, ManagedResource resource)
        {
            Id            = id;
            Name          = name ?? throw new ArgumentNullException(nameof(name));
            EnvironmentId = env.Id;
            ProviderId    = resource.Provider.Id;
            LocationId    = resource.LocationId;
            ResourceId    = resource.Id;
        }

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
    
        [Member("environmentId")]
        public long EnvironmentId { get; }

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        [Member("locationId")]
        public long LocationId { get; set; }

        ResourceType IManagedResource.ResourceType => ResourceType.Backend;

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