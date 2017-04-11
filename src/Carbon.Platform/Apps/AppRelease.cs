using System;

namespace Carbon.Platform.Apps
{
    using Data.Annotations;
    using Protection;
    using Versioning;

    [Dataset("AppReleases")]
    public class AppRelease
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

            AppId   = app.Id;
            Version = version;
            Sha256 = sha256;
            Digest  = new Hash(HashType.SHA256, sha256);
            CreatorId = creatorId;
        }

        [Member("appId"), Key]
        public long AppId { get; }

        [Member("version"), Key]
        public SemanticVersion Version { get; }

        // Replace with SHA256
        [Member("digest")]
        public Hash Digest { get; }

        [Member("buildId")]
        public long? BuildId { get; set; }

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