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

        public FrontendRelease(IFrontend frontend, SemanticVersion version)
        {
            #region Preconditions

            if (frontend == null)
                throw new ArgumentNullException(nameof(frontend));

            #endregion

            FrontendId = frontend.Id;
            FrontendName = frontend.Name;
            Version = version;
        }

        [Member("frontendId"), Key]
        public long FrontendId { get; set; }

        [Member("version"), Key]
        public SemanticVersion Version { get; set; }

        [Member("frontendName")]
        [StringLength(50)]
        public string FrontendName { get; set; }

        [Member("source")]
        public string Source { get; set; }

        [Member("activated"), Mutable]
        public DateTime? Activated { get; set; }

        [Member("digest")]
        public Hash Digest { get; set; }

        [Member("creatorId")]
        public long CreatorId { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        #region IFrontend

        long IFrontend.Id => FrontendId;

        string IFrontend.Name => FrontendName;

        SemanticVersion IFrontend.Version => Version;

        #endregion
    }
}
