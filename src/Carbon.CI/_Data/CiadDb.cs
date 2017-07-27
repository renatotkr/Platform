using System;

using Carbon.Data;

namespace Carbon.CI
{
    /// <summary>
    /// Continuous intergration & deployment
    /// </summary>
    public class CiadDb
    {
        public const string Name = "Ciad";

        public CiadDb(IDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));

            Builds             = new Dataset<Build, long>(context);
            BuildArtifacts     = new Dataset<BuildArtifact, long>(context);
            Projects           = new Dataset<ProjectInfo, long>(context);

            Deployments        = new Dataset<Deployment, long>(context);
            DeploymentTargets  = new Dataset<DeploymentTarget, (long, long)>(context);

            Repositories       = new Dataset<RepositoryInfo,   long>(context);
            RepositoryBranches = new Dataset<RepositoryBranch, long>(context);
            RepositoryCommits  = new Dataset<RepositoryCommit, long>(context);

            Packages           = new Dataset<PackageRecord, long>(context);
        }

        public IDbContext Context { get; }

        public Dataset<ProjectInfo,   long>  Projects { get; }
        public Dataset<PackageRecord, long>  Packages { get; }

        public Dataset<Build, long>          Builds            { get; }
        public Dataset<BuildArtifact, long>  BuildArtifacts    { get; }
        public Dataset<Deployment, long>     Deployments       { get; }

        public Dataset<DeploymentTarget, (long deploymentId, long hostId)> DeploymentTargets { get; }

        // Repositories ---------------------------------------------
        public Dataset<RepositoryInfo,   long> Repositories { get; }
        public Dataset<RepositoryBranch, long> RepositoryBranches { get; }
        public Dataset<RepositoryCommit, long> RepositoryCommits { get; }

    }
}
