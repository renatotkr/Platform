using System;

using Carbon.Data.Annotations;
using Carbon.Platform.CI;
using Carbon.Versioning;

namespace Carbon.Platform.Web
{
    [Dataset("WebsiteDeployments")]
    public class WebsiteDeployment
    {
        public WebsiteDeployment() { }

        public WebsiteDeployment(
            long id,
            long websiteId, 
            SemanticVersion revision, 
            long commitId, 
            long creatorId)
        {
            Id        = id;
            WebsiteId = websiteId;
            Revision  = revision.ToString();
            CommitId  = commitId;
            CreatorId = creatorId;
        }

        // environmentId + sequence
        [Member("id"), Key]
        public long Id { get; }

        [Member("status")]
        public DeploymentStatus Status { get; set; }

        [Member("websiteId")]
        public long WebsiteId { get; set; }

        [Member("revision")]
        public string Revision { get; }

        [Member("commitId")]
        public long CommitId { get; }

        [Member("creatorId")]
        public long CreatorId { get; }

        public long EnvironmentId => ScopedId.GetScope(Id);

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("completed")]
        public DateTime? Completed { get; set; }

        #endregion
    }
}
