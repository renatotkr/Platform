using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    [Dataset("Environments")]
    [DataIndex(IndexFlags.Unique, "appId", "type")]
    public class EnvironmentInfo : IEnvironment
    {
        public EnvironmentInfo() { }

        public EnvironmentInfo(
            long id, 
            long appId,
            EnvironmentType type,
            JsonObject variables = null)
        {
            Id        = id;
            AppId     = appId;
            Type      = type;
            Variables = variables;
        }
        
        // Applications are responsible for creating their environments
        [Member("id"), Key]
        public long Id { get; }

        [Member("appId")]
        public long AppId { get; }

        [Member("type")]
        public EnvironmentType Type { get; }

        [Member("name")]
        [StringLength(100)]
        public string Name { get; set; }

        [Member("variables")]
        [StringLength(1000)]
        public JsonObject Variables { get; }

        // Docker configuration?
        
        // The last deployment
        [Member("deploymentId")]
        public long DeploymentId { get; }

        [Member("revision")]
        public string Revision { get; set; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceType.Environment;

        #endregion

        #region Stats

        [Member("deploymentCount")]
        public int DeploymentCount { get; }

        // HostCount

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
