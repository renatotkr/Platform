using Carbon.Versioning;

namespace Carbon.Platform.CI
{
    public class DeployWebsiteRequest
    {
        public long WebsiteId { get; set; }

        public SemanticVersion WebsiteVersion { get; set; }

        public long EnvironmentId { get; set; }

        public long InitiatorId { get; set; }
    }
}