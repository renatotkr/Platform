using System;

using Carbon.Data.Annotations;
using Carbon.Versioning;

namespace Carbon.Platform.Web
{
    [Dataset("WebsiteReleases")]
    public class WebsiteRelease
    {
        public WebsiteRelease() { }

        public WebsiteRelease(long websiteId, SemanticVersion version)
        {
            WebsiteId = websiteId;
            Version = version;
        }

        public WebsiteRelease(IWebsite website, SemanticVersion version)
        {
            #region Preconditions

            if (website == null)
                throw new ArgumentNullException(nameof(website));

            #endregion

            WebsiteId   = website.Id;
            Version      = version;
        }

        [Member("websiteId"), Key]
        public long WebsiteId { get; set; }

        [Member("version"), Key]
        public SemanticVersion Version { get; set; }

        [Member("source")]
        public string Source { get; set; }

        [Member("commitId")] // repositoryId + sequence
        public long CommitId { get; set; }

        [Member("creatorId")]
        public long CreatorId { get; set; }

        #region Hashes

        [Member("sha256", TypeName = "binary(32)")]
        public byte[] Sha256 { get; set; }

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        #endregion
    }
}
