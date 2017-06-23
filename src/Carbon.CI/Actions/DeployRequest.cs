using System;
using Carbon.Platform;

using Carbon.Versioning;

namespace Carbon.CI
{
    public class DeployRequest
    {
        public DeployRequest() { }

        public DeployRequest(
            long programId, 
            SemanticVersion version,
            IEnvironment environment,
            long initiatorId)
        {
            ProgramId      = programId;
            ApplicationVersion = version;
            Environment        = environment ?? throw new ArgumentNullException(nameof(environment));
            InitiatorId        = initiatorId;
        }

        public long ProgramId { get; set; }

        public SemanticVersion ApplicationVersion { get; set; }
        
        public IEnvironment Environment { get; set; }

        public long InitiatorId { get; set; }
    }

    
    // Targets (environment, cluster, host)
}
