using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    [Dataset("Environments")]
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

            if (ownerId <= 0)
                throw new ArgumentException("Must be > 0", nameof(ownerId));

            #endregion

            Id      = id;
            Name    = name;
            Slug    = slug;
            OwnerId = ownerId;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }

        [Member("slug"), Unique]
        [StringLength(63)]
        public string Slug { get; }

        [Member("ownerId")]
        public long OwnerId { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; set; }

        public EnvironmentType Type
        {
            get
            {
                switch (Id % 4)
                {
                    case 0  : return EnvironmentType.Development;
                    case 1  : return EnvironmentType.Production;
                    case 2  : return EnvironmentType.Staging;
                    case 3  : return EnvironmentType.Intergration;
                    default : throw new Exception("unknown");
                }
            }
        }
        
        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Environment;

        #endregion

        #region Stats

        // the number of deployments made to the environment
        [Member("deploymentCount")]
        public int DeploymentCount { get; }

        // the number of commands issued against the environment
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
