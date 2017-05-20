using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.CI
{
    [Dataset("BuildProject")]
    public class BuildProject : IBuildProject
    {
        public BuildProject() { }

        public BuildProject(
            long id, 
            string name,
            ManagedResource resource)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            #endregion

            Id          = id;
            Name        = name;
            ProviderId  = resource.ProviderId;
            ResourceId  = resource.ResourceId;
            LocationId  = resource.LocationId;
        }

        [Member("id"), Key(sequenceName: "buildProjectId")]
        public long Id { get; }

        [Member("name")]
        public string Name { get; set; }

        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        [Member("ownerId")]
        public long OwnerId { get; set; }

        // Source
        // ...

        #region IResource

        [Member("providerId")]
        public int ProviderId { get; }

        [Member("resourceId")]
        public string ResourceId { get; }

        [Member("locationId")]
        public int LocationId { get; }
        
        ResourceType IResource.ResourceType => ResourceTypes.BuildProject; // ci:project

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [Member("started"), Mutable]
        public DateTime? Started { get; set; }

        [Member("completed"), Mutable]
        public DateTime? Completed { get; set; }

        #endregion
    }
}