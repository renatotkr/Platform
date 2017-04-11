using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;

namespace Carbon.Platform
{
    [Dataset("Environments")]
    [DataIndex(IndexFlags.Unique, "appId", "name")]
    public class AppEnvironment : IEnvironment
    {
        public AppEnvironment() { }

        public AppEnvironment(long id, long appId, string name, JsonObject variables = null)
        {
            Id        = id;
            AppId     = appId;
            Name      = name ?? throw new ArgumentNullException(nameof(name));
            Variables = variables;
        }

        // AppId + Sequence
        [Member("id"), Key]
        public long Id { get; }

        [Member("appId")]
        public long AppId { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }

        [Member("variables")]
        public JsonObject Variables { get; }
        
        [Member("deploymentId")]
        public long DeploymentId { get; }
        
        #region Stats

        [Member("deploymentCount")]
        public int DeploymentCount { get; set; } 

        #endregion

        #region Source

        [Member("repositoryId")]
        public long RepositoryId { get; set; }

        // AKA .git/HEAD -> refs/heads/master

        [Member("repositoryHead")]
        public string RepositoryHead { get; set; }

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