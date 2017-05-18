﻿using System;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.CI
{
    [Dataset("Builds")]
    public class Build : IBuild, IManagedResource
    {
        public Build() { }

        public Build(
            long id, 
            long commitId, 
            long creatorId,
            ManagedResource resource)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            #endregion

            Id        = id;
            CommitId  = commitId;
            CreatorId = creatorId;
            ProviderId = resource.ProviderId;
            ResourceId = resource.ResourceId;
            LocationId = resource.LocationId;
        }

        [Member("id"), Key(sequenceName: "buildId")]
        public long Id { get; }

        [Member("status"), Mutable]
        public BuildStatus Status { get; set; }
        
        // repositoryId + sequenceNumber
        [Member("commitId")]
        public long CommitId { get; }
        
        // InitiatorId ?

        [Member("creatorId")]
        public long CreatorId { get; }

        [Member("message"), Mutable]
        [StringLength(200)]
        public string Message { get; set; }

        [Member("duration"), Mutable]
        public TimeSpan? Duration { get; set; }

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