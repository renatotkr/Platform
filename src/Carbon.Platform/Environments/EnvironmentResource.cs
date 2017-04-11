using Carbon.Data.Annotations;

namespace Carbon.Platform
{
    [Dataset("EnvironmentResources")]
    public class EnvironmentResource
    {
        public EnvironmentResource() { }

        public EnvironmentResource(
            long id,
            long environmentId,
            long locationId,
            ResourceType resourceType,
            long resourceId
        )
        {
            Id            = id;
            EnvironmentId = environmentId;
            LocationId    = locationId;
            ResourceType  = resourceType;
            ResourceId    = resourceId;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("environmentId")]
        public long EnvironmentId { get; }

        [Member("locationId")]
        public long LocationId { get; }
        
        // e.g. Bucket | LoadBalancer | HostGroup ...
        [Member("resourceType")]
        public ResourceType ResourceType { get; }

        [Member("resourceId")]
        public long ResourceId { get; }
    }
}
