using System;

namespace Carbon.CI
{
    public interface IDeployment 
    {        
        long Id { get; }

        DeploymentStatus Status { get; }

        long CreatorId { get; }

        DateTime Created { get; }

        DateTime? Completed { get; }
    }
}