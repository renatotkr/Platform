using System;

using Carbon.Data.Annotations;
using Carbon.Platform.VersionControl;
using Carbon.Versioning;
using Carbon.Platform.CI;

namespace Carbon.Platform.Web
{
    [Dataset("WebsiteReleases")]
    [DataIndex(IndexFlags.Unique, "websiteId", "version")]
    public class WebsiteRelease : IRelease
    {
        public WebsiteRelease() { }

        public WebsiteRelease(
            long id,
            IWebsite website,
            SemanticVersion version, 
            byte[] sha256,
            IRepositoryCommit commit,
            long creatorId)
        {
            #region Preconditions

            if (website == null)
                throw new ArgumentNullException(nameof(website));

            if (sha256 == null)
                throw new ArgumentNullException(nameof(sha256));

            if (sha256.Length != 32)
                throw new ArgumentException("Must be 32", nameof(sha256));


            if (commit == null)
                throw new ArgumentNullException(nameof(commit));

            #endregion

            Id        = id;
            WebsiteId = website.Id;
            Version   = version;
            Sha256    = sha256 ?? throw new ArgumentNullException(nameof(sha256));
            CommitId  = commit.Id;
            CreatorId = creatorId;
        }

        // websiteId + sequenceNumber
        [Member("id"), Key]
        public long Id { get; }

        [Member("websiteId")]
        public long WebsiteId { get; }

        [Member("version")]
        public SemanticVersion Version { get; }

        // repositoryId + sequence
        [Member("commitId")] 
        public long CommitId { get; }

        [Member("creatorId")]
        public long CreatorId { get; }

        #region IRelease

        ReleaseType IRelease.Type => ReleaseType.Website;

        long IRelease.Id => Id;

        #endregion

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
