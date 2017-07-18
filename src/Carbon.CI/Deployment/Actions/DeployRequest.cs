﻿using System;
using Carbon.Platform.Computing;
using Carbon.Platform.Environments;

namespace Carbon.CI
{
    public class DeployRequest
    {
        public DeployRequest(
            IProgram program,
            IEnvironment environment,
            long initiatorId)
        {
            Program     = program     ?? throw new ArgumentNullException(nameof(program));
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public IProgram Program { get; }
        
        public IEnvironment Environment { get; }
    }
    
    // Targets (environment, cluster, host)
}
