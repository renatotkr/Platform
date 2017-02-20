using System;

namespace Carbon.Platform.Apps
{
    using Data.Annotations;
    using Protection;
    using Versioning;

    [Dataset("AppReleases")]
    public class AppRelease : IApp
    {
        public AppRelease() { }

        public AppRelease(IApp app, SemanticVersion version)
        {
            #region Preconditions

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (version == SemanticVersion.Zero)
                throw new ArgumentException("May not be 0.0.0", nameof(version));

            #endregion

            AppId = app.Id;
            AppName = app.Name;
            Version = version;
        }

        public AppRelease(long appId, SemanticVersion version)
        {
            #region Preconditions

            if (version == SemanticVersion.Zero)
                throw new ArgumentException("May not be 0.0.0", nameof(version));

            #endregion

            AppId = appId;
            Version = version;
        }

        [Member("appId"), Key]
        public long AppId { get; set; }

        [Member("version"), Key]
        public SemanticVersion Version { get; set; }

        [Member("appName")]
        [StringLength(50)]
        public string AppName { get; set; }

        [Member("buildId")]
        public long? BuildId { get; set; }

        [Member("creatorId")]
        public long CreatorId { get; set; }

        [Member("digest")]
        public Hash Digest { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        #region IApp

        long IApp.Id => AppId;

        string IApp.Name => AppName;

        #endregion

        // name@1.2.1
        public override string ToString() => AppName + "@" + Version;
    }
}