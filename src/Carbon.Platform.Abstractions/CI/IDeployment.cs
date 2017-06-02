using System;

using Carbon.Platform.Resources;

namespace Carbon.CI
{
    public interface IDeployment : IResource
    {        
        DeploymentStatus Status { get; }

        ReleaseType ReleaseType { get; }

        long ReleaseId { get; }

        long InitiatorId { get; }

        DateTime Created { get; }

        DateTime? Completed { get; }
    }
}