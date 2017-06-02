using System;

namespace Carbon.CI
{
    public interface IDeployment 
    {        
        long Id { get; }

        DeploymentStatus Status { get; }

        long InitiatorId { get; }

        DateTime Created { get; }

        DateTime? Completed { get; }
    }
}