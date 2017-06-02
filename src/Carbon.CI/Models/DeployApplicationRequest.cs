using System;
using Carbon.Platform;

using Carbon.Versioning;

namespace Carbon.CI
{
    public class DeployApplicationRequest
    {
        public DeployApplicationRequest() { }

        public DeployApplicationRequest(
            long applicationId, 
            SemanticVersion version,
            IEnvironment environment,
            long initiatorId)
        {
            ApplicationId      = applicationId;
            ApplicationVersion = version;
            Environment        = environment ?? throw new ArgumentNullException(nameof(environment));
            InitiatorId        = initiatorId;
        }

        public long ApplicationId { get; set; }

        public SemanticVersion ApplicationVersion { get; set; }
        
        public IEnvironment Environment { get; set; }

        public long InitiatorId { get; set; }
    }
    
    // Targets (environment, cluster, host)
}
