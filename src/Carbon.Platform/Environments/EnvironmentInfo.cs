using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Environments
{
    [Dataset("Environments")]
    [UniqueIndex("ownerId", "name")]
    public class EnvironmentInfo : IEnvironment
    {
        public EnvironmentInfo() { }

        public EnvironmentInfo(
            long id, 
            string name,
            long ownerId,
            string slug = null)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (name == null || string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            if (ownerId <= 0)
                throw new ArgumentException("Must be > 0", nameof(ownerId));

            #endregion

            Id      = id;
            Name    = name;
            Slug    = slug;
            OwnerId = ownerId;
        }

        [Member("id"), Key("environmentId")]
        public long Id { get; }

        [Member("ownerId")] // may change ownership...
        public long OwnerId { get; }

        [Member("name")]
        [StringLength(100)]
        public string Name { get; }

        [Member("slug"), Unique]
        [StringLength(63), Optional]
        public string Slug { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; set; }
        
        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Environment;

        #endregion

        #region Stats

        // the number of commands (including deployments) issued against the environment
        [Member("commandCount")]
        public int CommandCount { get; }

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}

// e.g. image.processor#production
