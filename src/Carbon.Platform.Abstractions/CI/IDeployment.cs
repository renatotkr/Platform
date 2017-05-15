using System;

using Carbon.Platform.Resources;

namespace Carbon.Platform.CI
{
    public interface IDeployment : IResource
    {        
        DeploymentStatus Status { get; }

        long CommitId { get; }

        long CreatorId { get; }

        DateTime Created { get; }

        DateTime? Completed { get; }
    }
}