﻿using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.CI
{
    [Dataset("Projects", Schema = CiadDb.Name)]
    [UniqueIndex("ownerId", "name")]
    public class ProjectInfo : IResource, IProject
    {
        public ProjectInfo() { }

        public ProjectInfo(
            long id, 
            string name,
            long repositoryId,
            long ownerId,
            ManagedResource resource,
            long imageId = 0,
            JsonObject properties = null)
        {
            Ensure.IsValidId(id);
            Ensure.IsValidId(repositoryId, nameof(repositoryId));
            Ensure.NotNullOrEmpty(name, nameof(name));
            Ensure.IsValidId(ownerId, nameof(ownerId));

            Id           = id;
            Name         = name;
            RepositoryId = repositoryId;
            ImageId      = imageId;
            OwnerId      = ownerId;
            ProviderId   = resource.ProviderId;
            ResourceId   = resource.ResourceId;
            LocationId   = resource.LocationId;
            Properties   = properties;
        }

        [Member("id"), Key(sequenceName: "projectId")]
        public long Id { get; }

        [Member("ownerId")]
        public long OwnerId { get; }

        [Member("name")]
        [Ascii, StringLength(100)]
        public string Name { get; }
        
        // Codebuild: [A-Za-z0-9][A-Za-z0-9\-_]{1,254}

        [Member("repositoryId"), Indexed]
        public long RepositoryId { get; }

        [Member("imageId")]
        public long ImageId { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region Stats

        [Member("buildCount")]
        public int BuildCount { get; }

        #endregion

        #region IResource

        [Member("providerId")]
        public int ProviderId { get; }

        [Member("resourceId")]
        [Ascii, StringLength(255)]
        public string ResourceId { get; }

        [Member("locationId")]
        public int LocationId { get; }
        
        ResourceType IResource.ResourceType => ResourceTypes.Project; // ci:project

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}