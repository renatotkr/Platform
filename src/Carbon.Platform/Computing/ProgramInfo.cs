﻿using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;
using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    [Dataset("Programs", Schema = "Computing")]
    [UniqueIndex("ownerId", "name")]
    public class ProgramInfo : IProgram, IResource
    {
        public ProgramInfo() { }

        public ProgramInfo(
            long id,   
            long ownerId,
            string name,
            string slug, 
            SemanticVersion version,
            string runtime = null,
            string[] addresses = null,
            ProgramType type = ProgramType.App,
            long? repositoryId = null,
            long? parentId = null,
            JsonObject properties = null)
        {
            Ensure.IsValidId(id);
            Ensure.IsValidId(ownerId, nameof(ownerId));
            Ensure.NotNullOrEmpty(name, nameof(name));

            if (name.Length > 63)
                throw new ArgumentException("Must be 63 characters or fewer", nameof(name));

            Id           = id;
            OwnerId      = ownerId;
            Name         = name;
            Slug         = slug;
            Type         = type;
            Runtime      = runtime;
            Addresses    = addresses;
            Version      = version;
            RepositoryId = repositoryId;
            ParentId     = parentId;
            Properties   = properties ?? new JsonObject();
        }

        [Member("id"), Key(sequenceName: "programId")]
        public long Id { get; }

        [Member("ownerId")]
        public long OwnerId { get; }

        [Member("type")]
        public ProgramType Type { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }
        
        // e.g. accelerator | ngnix | caddy
        [Member("slug"), Unique]
        [StringLength(63)]
        public string Slug { get; }

        [Member("runtime")]
        [StringLength(50)]
        public string Runtime { get; }

        [Member("addresses")]
        [StringLength(200)]
        public string[] Addresses { get; }

        [Member("parentId")]
        public long? ParentId { get; }

        [Member("repositoryId")]
        public long? RepositoryId { get; }

        [Member("version")]
        public SemanticVersion Version { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region Stats

        [Member("releaseCount")]
        public int ReleaseCount { get; }

        #endregion

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Program;

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