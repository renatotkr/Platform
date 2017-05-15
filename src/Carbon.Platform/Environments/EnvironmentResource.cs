using System;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    [Dataset("EnvironmentResources")]
    public class EnvironmentResource
    {
        public EnvironmentResource() { }

        public EnvironmentResource(
            long id,
            IEnvironment env,
            ILocation location,
            IResource resource
        )
        {
            #region Preconditions

            if (env == null)
                throw new ArgumentNullException(nameof(env));

            if (location == null)
                throw new ArgumentNullException(nameof(location));

            if (resource == null)
                throw new ArgumentNullException(nameof(resource));

            #endregion

            Id            = id;
            EnvironmentId = env.Id;
            LocationId    = location.Id;
            ResourceType  = resource.ResourceType;
            ResourceId    = resource.Id;
        }

        // environmentId + sequenceNumber
        [Member("id"), Key]
        public long Id { get; }

        [Member("environmentId")]
        public long EnvironmentId { get; }

        [Member("locationId")]
        public int LocationId { get; }
        
        // e.g. Bucket | LoadBalancer | HostGroup ...
        [Member("resourceType")]
        public ResourceType ResourceType { get; }

        [Member("resourceId")]
        public long ResourceId { get; }
    }
}