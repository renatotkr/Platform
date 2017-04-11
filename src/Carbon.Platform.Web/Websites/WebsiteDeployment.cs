using System;

using Carbon.Data.Annotations;
using Carbon.Platform.CI;
using Carbon.Platform.VersionControl;
using Carbon.Versioning;

namespace Carbon.Platform.Web
{
    [Dataset("WebsiteDeployments")]
    public class WebsiteDeployment : IDeployment
    {
        public WebsiteDeployment(long id, IWebsite website, SemanticVersion revision, ICommit commit, long creatorId)
        {
            #region Preconditions

            if (website == null)
                throw new ArgumentNullException(nameof(website));

            if (commit == null)
                throw new ArgumentNullException(nameof(commit));

            #endregion

            Id            = id;
            WebsiteId     = website.Id;
            Revision      = revision.ToString();
            CommitId      = commit.Id;
            CreatorId     = creatorId;
        }

        // environment + sequence
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
