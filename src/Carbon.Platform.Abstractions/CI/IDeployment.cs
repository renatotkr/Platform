using System;

namespace Carbon.Platform.CI
{
    public interface IDeployment
    {
        long Id { get; }
        
        DeploymentStatus Status { get; }

        long CommitId { get; }

        long CreatorId { get; }

        DateTime Created { get; }

        DateTime? Completed { get; }
    }

 
    // type?
    // Website | App
}