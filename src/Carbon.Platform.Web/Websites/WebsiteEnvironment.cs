using System;
using System.Runtime.Serialization;

using Carbon.Json;

namespace Carbon.Platform.Web
{
    using Data.Annotations;

    [Dataset("WebsiteEnvironments")]
    public class WebsiteEnvironment : IWebsiteEnvironment
    {
        public WebsiteEnvironment() { }

        public WebsiteEnvironment(long id, string name, long appEnvironmentId)
        {
            Id               = id;
            Name             = name ?? throw new ArgumentNullException(nameof(name));
            AppEnvironmentId = appEnvironmentId;
        }

        [Member("id"), Key] 
        public long Id { get; }
        
        // e.g. development, alpha, beta, stable

        [Member("name"), Key]
        [StringLength(63)]
        public string Name { get; }

        [Member("variables")]
        [StringLength(1000)]
        public JsonObject Variables { get; set; }

        [Member("appEnvironmentId")]
        public long AppEnvironmentId { get; }

        // e.g. master, 2.1.3
        [Member("releaseVersion")]
        public string ReleaseVersion { get; }

        public long WebsiteId => ScopedId.GetScope(Id);

        #region Repository

        [Member("repositoryId")]
        public long RepositoryId { get; set; }

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

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}