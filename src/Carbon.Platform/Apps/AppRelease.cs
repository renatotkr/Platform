using System;

using Carbon.Data.Annotations;
using Carbon.Versioning;
using Carbon.Platform.CI;

namespace Carbon.Platform.Apps
{
    [Dataset("AppReleases")]
    [DataIndex(IndexFlags.Unique, "appId", "version")]
    public class AppRelease : IAppRelease, IRelease
    {
        public AppRelease() { }

        public AppRelease(long id, long appId, SemanticVersion version, byte[] sha256, long creatorId)
        {
            #region Preconditions

            if (appId <= 0)
                throw new ArgumentException("Must be > 0", nameof(appId));

            if (version == SemanticVersion.Zero)
                throw new ArgumentException("May not be 0.0.0", nameof(version));

            if (sha256 == null)
                throw new ArgumentNullException(nameof(sha256));

            if (sha256.Length != 32)
                throw new ArgumentException("Must be 32 bytes", nameof(sha256));

            #endregion

            Id        = id;
            AppId     = appId;
            Version   = version;
            Sha256    = sha256;
            CreatorId = creatorId;
        }

        // appId + sequence
        [Member("id"), Key]
        public long Id { get; }

        [Member("appId")]
        public long AppId { get; }

        [Member("version")]
        public SemanticVersion Version { get; }

        [Member("buildId")]
        public long? BuildId { get; set; }

        [Member("commitId")]
        public long CommitId { get; set; }

        [Member("creatorId")]
        public long CreatorId { get; }

        #region IDeployable

        ReleaseType IRelease.Type => ReleaseType.Application;

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