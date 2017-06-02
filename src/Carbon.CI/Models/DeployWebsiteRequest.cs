using System;

using Carbon.Platform;
using Carbon.Platform.Web;
using Carbon.Versioning;

namespace Carbon.CI
{
    public class DeployWebsiteRequest
    {
        public DeployWebsiteRequest() { }

        public DeployWebsiteRequest(
            WebsiteRelease release, 
            IEnvironment environment, 
            long initiatorId)
        {
            #region Preconditions

            if (release == null)
                throw new ArgumentNullException(nameof(release));

            #endregion

            WebsiteId      = release.WebsiteId;
            WebsiteVersion = release.Version;
            Environment    = environment ?? throw new ArgumentNullException(nameof(environment));
            InitiatorId    = initiatorId;
        }

        public long WebsiteId { get; set; }

        public SemanticVersion WebsiteVersion { get; set; }

        public IEnvironment Environment { get; set; }

        public long InitiatorId { get; set; }
    }
}