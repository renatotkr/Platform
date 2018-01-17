using System;

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
            long ownerId,
            string name,
            string slug = null,
            JsonObject properties = null)
        {
            Ensure.IsValidId(id);
            Ensure.IsValidId(ownerId, nameof(ownerId));
            Ensure.NotNullOrEmpty(name, nameof(name));

            if (name.Length > 63)
                throw new ArgumentException("Must be 63 characters or fewer", nameof(name));

            Id         = id;
            OwnerId    = ownerId;
            Name       = name;
            Slug       = slug;
            Properties = properties ?? new JsonObject();
        }

        [Member("id"), Key("environmentId")]
        public long Id { get; }

        [Member("ownerId")] // may change ownership...
        public long OwnerId { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }

        [Member("slug"), Unique]
        [StringLength(63), Optional]
        public string Slug { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Environment;

        #endregion

        #region Timestamps

        [Member("activated")]
        public DateTime? Activated { get; }

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }
        
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion

        #region Helpers

        public bool IsActivated => Activated != null;

        #endregion

    }
}

/*
processor#production
*/