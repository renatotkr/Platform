using System;

using Carbon.Data.Annotations;
using Carbon.Versioning;

namespace Carbon.Platform.Apps
{
    [Dataset("AppReleases")]
    public class AppRelease : IAppRelease
    {
        public AppRelease() { }

        public AppRelease(IApp app, SemanticVersion version, byte[] sha256, long creatorId)
        {
            #region Preconditions

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (version == SemanticVersion.Zero)
                throw new ArgumentException("May not be 0.0.0", nameof(version));

            #endregion

            AppId     = app.Id;
            Version   = version;
            Sha256    = sha256;
            CreatorId = creatorId;
        }

        [Member("appId"), Key]
        public long AppId { get; }

        [Member("version"), Key]
        public SemanticVersion Version { get; }

        [Member("buildId")]
        public long? BuildId { get; set; }

        [Member("commitId")]
        public long CommitId { get; set; }

        [Member("creatorId")]
        public long CreatorId { get; }

        #region Hashes

        [Member("sha256", TypeName = "binary(32)")]
        public byte[] Sha256 { get; set; }

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        #endregion
    }
}