using System;

using Carbon.Data.Annotations;
using Carbon.Platform.VersionControl;
using Carbon.Versioning;

namespace Carbon.Platform.Web
{
    [Dataset("WebsiteReleases")]
    public class WebsiteRelease
    {
        public WebsiteRelease() { }

        public WebsiteRelease(IWebsite website, SemanticVersion version, byte[] sha256, ICommit commit, long creatorId)
        {
            #region Preconditions

            if (website == null)
                throw new ArgumentNullException(nameof(website));

            if (commit == null)
                throw new ArgumentNullException(nameof(commit));

            #endregion

            WebsiteId = website.Id;
            Version   = version;
            Sha256    = sha256 ?? throw new ArgumentNullException(nameof(sha256));
            CommitId  = commit.Id;
            CreatorId = creatorId;
        }

        [Member("websiteId"), Key]
        public long WebsiteId { get; }

        [Member("version"), Key]
        public SemanticVersion Version { get; }

        // repositoryId + sequence
        [Member("commitId")] 
        public long CommitId { get; }

        [Member("creatorId")]
        public long CreatorId { get; }

        #region Hashes

        [Member("sha256", TypeName = "binary(32)")]
        public byte[] Sha256 { get; }

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        #endregion
    }
}
