using System;

using Carbon.Data.Annotations;
using Carbon.Platform.Storage;
using Carbon.Versioning;
using Carbon.CI;
using Carbon.Json;

namespace Carbon.Platform.Web
{
    [Dataset("WebsiteReleases")]
    [UniqueIndex("websiteId", "version")]
    public class WebsiteRelease : IRelease, IWebsite
    {
        public WebsiteRelease() { }

        public WebsiteRelease(
            long id,
            IWebsite website,
            SemanticVersion version, 
            JsonObject properties,
            IRepositoryCommit commit,
            long creatorId)
        {
            #region Preconditions

            if (website == null)
                throw new ArgumentNullException(nameof(website));

            if (commit == null)
                throw new ArgumentNullException(nameof(commit));

            #endregion

            Id         = id;
            WebsiteId  = website.Id;
            Version    = version;
            CommitId   = commit.Id;
            CreatorId  = creatorId;
            Properties = properties;
        }

        // websiteId + sequenceNumber
        [Member("id"), Key]
        public long Id { get; }

        [Member("websiteId")]
        public long WebsiteId { get; }

        [Member("websiteName")]
        public string WebsiteName { get; }

        [Member("version")]
        public SemanticVersion Version { get; }

        [Member("commitId")] 
        public long CommitId { get; }

        [Member("creatorId")]
        public long CreatorId { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region IRelease

        ReleaseType IRelease.Type => ReleaseType.Website;

        #endregion

        #region IWebsite

        string IWebsite.Name => WebsiteName;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        #endregion
    }
}
