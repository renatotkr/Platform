using System;

namespace Carbon.Platform.Frontends
{
    using Data.Annotations;
    using Versioning;

    [Dataset("FrontendReleases")]
    public class FrontendRelease
    {
        public FrontendRelease() { }

        public FrontendRelease(long frontendId, SemanticVersion version)
        {
            FrontendId = frontendId;
            Version = version;
        }

        [Member("frontendId"), Key]
        public long FrontendId { get; set; }

        [Member("version"), Key]
        public SemanticVersion Version { get; set; }
        
        // Name?

        [Member("source")]
        public string Source { get; set; }

        [Member("activated"), Mutable]
        public DateTime? Activated { get; set; }

        [Member("creatorId")]
        public long CreatorId { get; set; }

        [Member("created")]
        public DateTime Created { get; set; }

        /*
        #region Helpers

        [IgnoreDataMember]
        public object Creator { get; set; }

        [IgnoreDataMember]
        public string Path => FrontendId + "/" + Version.ToString();

        // 1/1.0.2

        #endregion
        */
    }
}
