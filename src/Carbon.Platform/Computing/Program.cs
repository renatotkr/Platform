﻿using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [Dataset("Programs", Schema = "Computing")]
    [DataIndex(IndexFlags.Unique, "ownerId", "name")]
    public class Program : IProgram, IResource
    {
        public Program() { }

        public Program(
            long id, 
            string name, 
            string slug, 
            long ownerId,
            ProgramType type = ProgramType.Application)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (ownerId <= 0)
                throw new ArgumentException("Must be > 0", nameof(ownerId));

            #endregion

            Id      = id;
            Name    = name ?? throw new ArgumentNullException(nameof(name));
            Slug    = slug;
            Type    = type;
            OwnerId = ownerId;
        }

        [Member("id"), Key(sequenceName: "programId", increment: 4)]
        public long Id { get; }

        [Member("type")]
        public ProgramType Type { get; }

        [Member("ownerId")]
        [Indexed]
        public long OwnerId { get; }

        [Member("name")]
        public string Name { get; }
        
        // e.g. accelerator | ngnix
        [Member("slug"), Unique]
        [StringLength(63)]
        public string Slug { get; }

        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        #region Stats

        [Member("releaseCount")]
        public int ReleaseCount { get; }

        #endregion

        #region Resource

        [Member("repositoryId")]
        public long RepositoryId { get; set; }

        #endregion

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Program;

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