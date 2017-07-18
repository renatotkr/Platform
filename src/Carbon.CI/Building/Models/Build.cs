﻿using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.CI
{
    [Dataset("Builds", Schema = CiadDb.Name)]
    public class Build : IBuild, IManagedResource
    {
        public Build() { }

        public Build(
            long id, 
            long commitId, 
            long creatorId,
            ManagedResource resource,
            JsonObject properties = null)
        {
            #region Preconditions

            Validate.Id(id);
            
            Validate.Id(creatorId, nameof(creatorId));

            #endregion

            Id         = id;
            CommitId   = commitId;
            CreatorId  = creatorId;
            ProviderId = resource.ProviderId;
            ResourceId = resource.ResourceId;
            LocationId = resource.LocationId;
            Properties = properties;
        }

        // projectId | #
        [Member("id"), Key]
        public long Id { get; }
        
        [Member("status"), Mutable]
        public BuildStatus Status { get; set; }

        [Member("phase"), Mutable]
        [StringLength(50)]
        public string Phase { get; set; }

        [Member("commitId")]
        public long CommitId { get; }
        
        [Member("creatorId")]
        public long CreatorId { get; }

        [Member("message"), Mutable]
        [StringLength(200)]
        public string Message { get; set; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region IResource

        [Member("providerId")]
        public int ProviderId { get; }

        [Member("resourceId")]
        public string ResourceId { get; }

        [Member("locationId")]
        public int LocationId { get; }
        
        ResourceType IResource.ResourceType => ResourceTypes.Build;

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