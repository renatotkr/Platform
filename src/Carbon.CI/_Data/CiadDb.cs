using System;

using Carbon.Data;

namespace Carbon.CI
{
    /// <summary>
    /// Continuous intergration & deployment
    /// </summary>
    public class CiadDb
    {
        public CiadDb(IDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));

            Builds                = new Dataset<Build, long>(context);
            BuildArtifacts        = new Dataset<BuildArtifact, long>(context);
            Projects              = new Dataset<ProjectInfo, long>(context);

            Deployments           = new Dataset<Deployment, long>(context);
            DeploymentTargets     = new Dataset<DeploymentTarget, (long, long)>(context);
        }

        public IDbContext Context { get; }

        public Dataset<Build, long>                    Builds            { get; }
        public Dataset<BuildArtifact, long>            BuildArtifacts    { get; }
        public Dataset<Deployment, long>               Deployments       { get; }
        public Dataset<DeploymentTarget, (long, long)> DeploymentTargets { get; }
        public Dataset<ProjectInfo, long>              Projects { get; }

    }
}
