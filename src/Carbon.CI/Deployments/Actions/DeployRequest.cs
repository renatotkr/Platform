using System;
using Carbon.Platform.Computing;
using Carbon.Platform.Environments;

namespace Carbon.CI
{
    public class DeployRequest
    {
        public DeployRequest(IProgram program, IEnvironment environment)
        {
            Ensure.NotNull(program,     nameof(program));
            Ensure.NotNull(environment, nameof(environment));

            Program = program;
            Environment = environment;
        }

        public IProgram Program { get; }
        
        public IEnvironment Environment { get; }

        // TODO: Targets
    }
    
    // Targets (environment, cluster, host)
}
