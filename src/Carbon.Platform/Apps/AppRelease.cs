using System;

namespace Carbon.Platform.Apps
{
    using Data.Annotations;
    using Versioning;

    [Dataset("AppReleases")]
    public class AppRelease : IApp
    {
        public AppRelease() { }

        public AppRelease(long appId, SemanticVersion version)
        {
            AppId      = appId;
            Version = version;
        }

        [Member("appId"), Key]
        public long AppId { get; set; }

        [Member("version"), Key]
        public SemanticVersion Version { get; set; }

        [Member("name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Member("buildId")]
        public long? BuildId { get; set; }

        // Build Digest?

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }


        #region IApp

        long IApp.Id => AppId;

        #endregion

        // name@1.2.1
        public override string ToString() => Name + "@" + Version;
    }
}