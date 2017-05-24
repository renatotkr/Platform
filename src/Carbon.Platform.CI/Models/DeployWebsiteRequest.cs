using System;

using Carbon.Platform.Web;
using Carbon.Versioning;

namespace Carbon.Platform.CI
{
    public class DeployWebsiteRequest
    {
        public DeployWebsiteRequest() { }

        public DeployWebsiteRequest(WebsiteRelease release, long initiatorId)
        {
            #region Preconditions

            if (release == null)
                throw new ArgumentNullException(nameof(release));

            #endregion

            WebsiteId = release.WebsiteId;
            WebsiteVersion = release.Version;
            InitiatorId = initiatorId;
        }

        public long WebsiteId { get; set; }

        public SemanticVersion WebsiteVersion { get; set; }

        public long EnvironmentId { get; set; }

        public long InitiatorId { get; set; }
    }
}