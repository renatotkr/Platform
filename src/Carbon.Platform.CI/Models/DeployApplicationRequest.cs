using Carbon.Versioning;

namespace Carbon.Platform.CI
{
    public class DeployApplicationRequest
    {
        public long ApplicationId { get; set; }

        public SemanticVersion ApplicationVersion { get; set; }

        public long EnvironmentId { get; set; }

        public long CreatorId { get; set; }
    }
}
