using System;

namespace Carbon.Platform.Frontends
{
    using Data.Annotations;
    using Protection;
    using Versioning;

    [Dataset("FrontendReleases")]
    public class FrontendRelease : IFrontend
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

        [Member("name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Member("source")]
        public string Source { get; set; }

        [Member("activated"), Mutable]
        public DateTime? Activated { get; set; }

        [Member("digest")]
        public Hash Digest { get; set; }

        [Member("creatorId")]
        public long CreatorId { get; set; }

        [Member("created")]
        public DateTime Created { get; set; }

        #region Frontend

        long IFrontend.Id => FrontendId;

        SemanticVersion IFrontend.Version => Version;

        #endregion


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
