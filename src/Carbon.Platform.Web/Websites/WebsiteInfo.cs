using System;
using System.Runtime.Serialization;

using Carbon.Json;
using Carbon.Data.Annotations;

namespace Carbon.Platform.Web
{
    [Dataset("Websites")]
    [DataIndex(IndexFlags.Unique, "ownerId", "name")]
    public class WebsiteInfo : IWebsite
    {
        public WebsiteInfo() { }

        public WebsiteInfo(long id, string name, long repositoryId, long ownerId)
        {
            Id           = id;
            Name         = name ?? throw new ArgumentNullException(nameof(name));
            RepositoryId = repositoryId;
            OwnerId      = ownerId;
        }

        [Member("id"), Key] 
        public long Id { get; }
        
        [IgnoreDataMember]
        [Member("ownerId")]
        public long OwnerId { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }

        [Member("variables")]
        [StringLength(1000)]
        public JsonObject Variables { get; set; }

        // Revision?

        [Member("repositoryId")]
        public long RepositoryId { get; }

        [Member("deploymentId")]
        public long DeploymentId { get; set; }

        #region Environment

        [Member("environmentId")]
        public long EnvironmentId { get; set; }

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }
        
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion

        public override string ToString() => Name;
    }
}

// Notes:
// Themes are websites too

// TODO: Verify repository head format
// https://git-scm.com/docs/git-check-ref-format
